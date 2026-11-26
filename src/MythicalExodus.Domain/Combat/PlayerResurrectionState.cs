namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Statistics;

public enum ResurrectionChargeKind { Initial = 0, Additional = 1 }
public enum PlayerResurrectionPhase { Alive = 0, AwaitingDecision = 1, GameOver = 2 }

public sealed class ResurrectionDebuffDefinition
{
    private readonly StatModifier[] _modifiers;
    private readonly IReadOnlyList<StatModifier> _readOnlyModifiers;

    public ResurrectionDebuffDefinition(SimulationDuration duration, IEnumerable<StatModifier> modifiers)
    {
        if (duration.Ticks <= 0)
            throw new ArgumentOutOfRangeException(nameof(duration), "A resurrection debuff must have a positive duration.");
        ArgumentNullException.ThrowIfNull(modifiers);
        _modifiers = modifiers.ToArray();
        if (_modifiers.Length == 0 || _modifiers.Any(modifier => modifier is null))
            throw new ArgumentException("A resurrection debuff requires at least one stat modifier.", nameof(modifiers));
        if (_modifiers.Select(modifier => modifier.Id).Distinct().Count() != _modifiers.Length)
            throw new ArgumentException("Resurrection debuff modifier identifiers must be unique.", nameof(modifiers));
        Duration = duration;
        _readOnlyModifiers = Array.AsReadOnly(_modifiers);
    }

    public SimulationDuration Duration { get; }
    public IReadOnlyList<StatModifier> Modifiers => _readOnlyModifiers;
    public TemporaryResurrectionDebuff ActivateAt(SimulationInstant instant) => new(this, instant, instant.Add(Duration));
}

public sealed record TemporaryResurrectionDebuff
{
    internal TemporaryResurrectionDebuff(ResurrectionDebuffDefinition definition, SimulationInstant startedAt, SimulationInstant expiresAt)
    {
        Definition = definition;
        StartedAt = startedAt;
        ExpiresAt = expiresAt;
    }

    public ResurrectionDebuffDefinition Definition { get; }
    public SimulationInstant StartedAt { get; }
    public SimulationInstant ExpiresAt { get; }
    public bool IsActiveAt(SimulationInstant instant) => instant.Tick < ExpiresAt.Tick;
    public IReadOnlyList<StatModifier> ModifiersAt(SimulationInstant instant) =>
        IsActiveAt(instant) ? Definition.Modifiers : Array.Empty<StatModifier>();
}

public sealed record PlayerResurrectionState
{
    public PlayerResurrectionState(ResurrectionState charges, EntityId lastRestPointId)
    {
        ArgumentNullException.ThrowIfNull(charges);
        if (lastRestPointId.IsEmpty)
            throw new ArgumentException("A valid last rest point identifier is required.", nameof(lastRestPointId));
        Charges = charges;
        LastRestPointId = lastRestPointId;
        Phase = PlayerResurrectionPhase.Alive;
    }

    private PlayerResurrectionState(ResurrectionState charges, EntityId lastRestPointId, PlayerResurrectionPhase phase, TemporaryResurrectionDebuff? activeDebuff)
    {
        Charges = charges;
        LastRestPointId = lastRestPointId;
        Phase = phase;
        ActiveDebuff = activeDebuff;
    }

    public ResurrectionState Charges { get; }
    public EntityId LastRestPointId { get; }
    public PlayerResurrectionPhase Phase { get; }
    public TemporaryResurrectionDebuff? ActiveDebuff { get; }
    public bool CanChooseResurrection =>
        Phase == PlayerResurrectionPhase.AwaitingDecision &&
        (Charges.InitialChargeAvailable || Charges.AdditionalUsableCharges > 0);

    /// <summary>Connects resolved player damage to the voluntary resurrection decision.</summary>
    public Result<PlayerResurrectionState> TryProcessCombatImpact(CombatImpactResult impact)
    {
        ArgumentNullException.ThrowIfNull(impact);
        if (Phase != PlayerResurrectionPhase.Alive)
            return Failure("resurrection.invalid_phase", "Combat impacts can only be processed while the player is alive.");
        return impact.VitalityDepleted
            ? TryBeginLethalEvent()
            : Result<PlayerResurrectionState>.Success(this);
    }

    public Result<PlayerResurrectionState> TryBeginLethalEvent()
    {
        if (Phase != PlayerResurrectionPhase.Alive)
            return Failure("resurrection.invalid_phase", "A lethal event can only begin while the player is alive.");
        var nextPhase = Charges.InitialChargeAvailable || Charges.AdditionalUsableCharges > 0
            ? PlayerResurrectionPhase.AwaitingDecision
            : PlayerResurrectionPhase.GameOver;
        return Result<PlayerResurrectionState>.Success(
            new PlayerResurrectionState(Charges, LastRestPointId, nextPhase, null));
    }

    public Result<PlayerResurrectionState> TryResurrect(ResurrectionChargeKind chargeKind, ResurrectionDebuffDefinition debuff, SimulationInstant now)
    {
        if (Phase != PlayerResurrectionPhase.AwaitingDecision)
            return Failure("resurrection.decision_not_pending", "There is no pending resurrection decision.");
        ArgumentNullException.ThrowIfNull(debuff);
        var consumption = chargeKind switch
        {
            ResurrectionChargeKind.Initial => Charges.TryConsumeInitialCharge(),
            ResurrectionChargeKind.Additional => Charges.TryConsumeAdditionalCharge(),
            _ => null
        };
        if (consumption is null)
            return Failure("resurrection.charge_kind_invalid", "The selected resurrection charge type is invalid.");
        if (consumption.IsFailure)
            return Result<PlayerResurrectionState>.Failure(consumption.Error!);
        return Result<PlayerResurrectionState>.Success(new PlayerResurrectionState(
            consumption.Value, LastRestPointId, PlayerResurrectionPhase.Alive, debuff.ActivateAt(now)));
    }

    public Result<PlayerResurrectionState> TryAcceptDeath()
    {
        if (Phase != PlayerResurrectionPhase.AwaitingDecision)
            return Failure("resurrection.decision_not_pending", "There is no pending death decision.");
        return Result<PlayerResurrectionState>.Success(
            new PlayerResurrectionState(Charges, LastRestPointId, PlayerResurrectionPhase.GameOver, null));
    }

    public Result<PlayerResurrectionState> TryRestAt(EntityId restPointId)
    {
        if (Phase != PlayerResurrectionPhase.Alive)
            return Failure("resurrection.rest_requires_alive_player", "The player must be alive to use a rest point.");
        if (restPointId.IsEmpty)
            return Failure("resurrection.rest_point_invalid", "A valid rest point identifier is required.");
        return Result<PlayerResurrectionState>.Success(new PlayerResurrectionState(
            Charges.ReturnToRestPoint(), restPointId, PlayerResurrectionPhase.Alive, ActiveDebuff));
    }

    public Result<PlayerResurrectionState> TryReturnAfterGameOver()
    {
        if (Phase != PlayerResurrectionPhase.GameOver)
            return Failure("resurrection.game_over_not_active", "The player can only return to a rest point after Game Over.");
        return Result<PlayerResurrectionState>.Success(new PlayerResurrectionState(
            Charges.ReturnToRestPoint(), LastRestPointId, PlayerResurrectionPhase.Alive, null));
    }

    public Result<PlayerResurrectionState> TryRecordEnemyDefeat(decimal progress)
    {
        if (Phase != PlayerResurrectionPhase.Alive)
            return Failure("resurrection.enemy_progress_requires_alive_player", "Enemy defeat progress can only be recorded while the player is alive.");
        var result = Charges.TryRecordEnemyDefeat(progress);
        if (result.IsFailure)
            return Result<PlayerResurrectionState>.Failure(result.Error!);
        return Result<PlayerResurrectionState>.Success(
            new PlayerResurrectionState(result.Value, LastRestPointId, Phase, ActiveDebuff));
    }

    public Result<PlayerResurrectionState> TryUnlockAdditionalOpportunity()
    {
        if (Phase != PlayerResurrectionPhase.Alive)
            return Failure("resurrection.unlock_requires_alive_player", "An opportunity can only be unlocked while the player is alive.");
        var result = Charges.TryUnlockAdditionalOpportunity();
        if (result.IsFailure)
            return Result<PlayerResurrectionState>.Failure(result.Error!);
        return Result<PlayerResurrectionState>.Success(
            new PlayerResurrectionState(result.Value, LastRestPointId, Phase, ActiveDebuff));
    }

    public PlayerResurrectionState AdvanceTo(SimulationInstant now)
    {
        if (ActiveDebuff is null || ActiveDebuff.IsActiveAt(now))
            return this;
        return new PlayerResurrectionState(Charges, LastRestPointId, Phase, null);
    }

    private static Result<PlayerResurrectionState> Failure(string code, string message) =>
        Result<PlayerResurrectionState>.Failure(new GameError(code, message));
}