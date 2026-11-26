namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

public enum BossPhaseTransitionStatus
{
    Active = 0,
    VulnerabilityWindow = 1,
    InvulnerableTransition = 2,
    Defeated = 3
}

/// <summary>Content profile for a boss with three to five permanent vitality phases.</summary>
public sealed class BossPhaseProfile
{
    private readonly HashSet<int> _specialTransitionAttackPhases;
    private readonly IReadOnlySet<int> _readOnlySpecialTransitionAttackPhases;

    public BossPhaseProfile(int totalPhases, IEnumerable<int>? specialTransitionAttackPhases = null)
    {
        if (totalPhases is < 3 or > 5)
            throw new ArgumentOutOfRangeException(nameof(totalPhases), "Bosses must have between three and five phases.");

        _specialTransitionAttackPhases = specialTransitionAttackPhases?.ToHashSet() ?? [];
        if (_specialTransitionAttackPhases.Any(phase => phase < 1 || phase > totalPhases))
            throw new ArgumentException("Special transition attack phases must belong to this boss profile.", nameof(specialTransitionAttackPhases));

        _readOnlySpecialTransitionAttackPhases = new System.Collections.ObjectModel.ReadOnlySet<int>(_specialTransitionAttackPhases);
        TotalPhases = totalPhases;
    }

    public int TotalPhases { get; }
    public IReadOnlySet<int> SpecialTransitionAttackPhases => _readOnlySpecialTransitionAttackPhases;
    public bool HasSpecialTransitionAttack(int phase) => _specialTransitionAttackPhases.Contains(phase);
}

/// <summary>
/// Immutable phase and transition state. The application layer supplies the
/// resource state after a bar loss and advances it using logical simulation time.
/// </summary>
public sealed record BossPhaseState
{
    public const decimal PostureRetentionAfterPhaseChange = 0.40m;

    public BossPhaseState(BossPhaseProfile profile, CombatantResourceState resources)
    {
        ArgumentNullException.ThrowIfNull(profile);
        ArgumentNullException.ThrowIfNull(resources);
        if (resources.Vitality.Bars.Count != profile.TotalPhases)
            throw new ArgumentException("Initial boss Vitality bar count must match its phase count.", nameof(resources));

        Profile = profile;
        Resources = resources;
        CurrentPhase = 1;
        Status = BossPhaseTransitionStatus.Active;
    }

    private BossPhaseState(
        BossPhaseProfile profile,
        CombatantResourceState resources,
        int currentPhase,
        BossPhaseTransitionStatus status,
        SimulationInstant? vulnerabilityEndsAt,
        SimulationInstant? transitionEndsAt)
    {
        Profile = profile;
        Resources = resources;
        CurrentPhase = currentPhase;
        Status = status;
        VulnerabilityEndsAt = vulnerabilityEndsAt;
        TransitionEndsAt = transitionEndsAt;
    }

    public BossPhaseProfile Profile { get; }
    public CombatantResourceState Resources { get; }
    public int CurrentPhase { get; }
    public BossPhaseTransitionStatus Status { get; }
    public SimulationInstant? VulnerabilityEndsAt { get; }
    public SimulationInstant? TransitionEndsAt { get; }
    public bool IsVulnerable => Status == BossPhaseTransitionStatus.VulnerabilityWindow;
    public bool IsInvulnerable => Status == BossPhaseTransitionStatus.InvulnerableTransition;
    public bool CanUseSpecialTransitionAttack =>
        IsInvulnerable && Profile.HasSpecialTransitionAttack(CurrentPhase);

    /// <summary>Processes exactly one permanently depleted boss bar and begins the next phase.</summary>
    public Result<BossPhaseState> TryAdvanceAfterBarDepletion(
        CombatantResourceState resourcesAfterBarLoss,
        SimulationInstant now,
        SimulationDuration vulnerabilityDuration,
        SimulationDuration invulnerableTransitionDuration)
    {
        ArgumentNullException.ThrowIfNull(resourcesAfterBarLoss);
        if (Status != BossPhaseTransitionStatus.Active)
            return Failure("combat.boss.phase_not_active", "A boss phase can advance only during active combat.");
        if (vulnerabilityDuration.Ticks <= 0 || invulnerableTransitionDuration.Ticks <= 0)
            return Failure("combat.boss.transition_duration_invalid", "Boss vulnerability and transition durations must be positive.");

        var expectedRemainingBars = Profile.TotalPhases - CurrentPhase;
        if (resourcesAfterBarLoss.Vitality.Bars.Count != expectedRemainingBars)
        {
            return Failure(
                "combat.boss.invalid_bar_transition",
                "A boss phase transition must account for exactly one permanently depleted Vitality bar.");
        }

        if (resourcesAfterBarLoss.Vitality.IsDepleted)
        {
            return Result<BossPhaseState>.Success(new BossPhaseState(
                Profile,
                resourcesAfterBarLoss,
                CurrentPhase,
                BossPhaseTransitionStatus.Defeated,
                null,
                null));
        }

        if (CurrentPhase >= Profile.TotalPhases)
            return Failure("combat.boss.phase_limit_reached", "The boss has no remaining phase to enter.");

        var retainedPosture = resourcesAfterBarLoss.Posture.Current * PostureRetentionAfterPhaseChange;
        var postureUpdate = resourcesAfterBarLoss.TrySetPostureValue(retainedPosture);
        if (postureUpdate.IsFailure)
            return Result<BossPhaseState>.Failure(postureUpdate.Error!);

        var vulnerabilityEndsAt = now.Add(vulnerabilityDuration);
        var transitionEndsAt = vulnerabilityEndsAt.Add(invulnerableTransitionDuration);
        return Result<BossPhaseState>.Success(new BossPhaseState(
            Profile,
            postureUpdate.Value,
            CurrentPhase + 1,
            BossPhaseTransitionStatus.VulnerabilityWindow,
            vulnerabilityEndsAt,
            transitionEndsAt));
    }

    /// <summary>Advances vulnerability and invulnerability windows on the logical timeline.</summary>
    public BossPhaseState AdvanceTo(SimulationInstant now)
    {
        if (Status == BossPhaseTransitionStatus.VulnerabilityWindow &&
            VulnerabilityEndsAt.HasValue &&
            now.Tick >= VulnerabilityEndsAt.Value.Tick)
        {
            if (TransitionEndsAt.HasValue && now.Tick >= TransitionEndsAt.Value.Tick)
            {
                return new BossPhaseState(
                    Profile, Resources, CurrentPhase, BossPhaseTransitionStatus.Active, null, null);
            }

            return new BossPhaseState(
                Profile, Resources, CurrentPhase, BossPhaseTransitionStatus.InvulnerableTransition,
                VulnerabilityEndsAt, TransitionEndsAt);
        }

        if (Status == BossPhaseTransitionStatus.InvulnerableTransition &&
            TransitionEndsAt.HasValue &&
            now.Tick >= TransitionEndsAt.Value.Tick)
        {
            return new BossPhaseState(
                Profile, Resources, CurrentPhase, BossPhaseTransitionStatus.Active, null, null);
        }

        return this;
    }

    private static Result<BossPhaseState> Failure(string code, string message) =>
        Result<BossPhaseState>.Failure(new GameError(code, message));
}
