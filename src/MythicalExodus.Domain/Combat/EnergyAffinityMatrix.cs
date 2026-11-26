namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

public enum EnergyType
{
    Aura = 1,
    Mana = 2,
    Prana = 3,
    NaturalEnergy = 4,
    AbyssalEnergy = 5,
    AstralPower = 6
}

public enum EnergyAffinityRelation
{
    Neutral = 0,
    Advantage = 1,
    Disadvantage = 2,
    Reciprocal = 3
}

public sealed record EnergyAffinityResult(
    EnergyType AttackerEnergy,
    EnergyType DefenderEnergy,
    EnergyAffinityRelation Relation,
    decimal Multiplier,
    decimal DamageBeforeAffinity,
    decimal DamageAfterAffinity);

/// <summary>
/// Versioned directed affinity matrix transcribed from the approved six-energy diagram.
/// Unlisted relationships are neutral; reciprocal pairs override ordinary directionality.
/// </summary>
public sealed class EnergyAffinityMatrix
{
    public const decimal AdvantageMultiplier = 1.50m;
    public const decimal DisadvantageMultiplier = 0.75m;
    public const decimal ReciprocalMultiplier = 1.25m;
    public const decimal NeutralMultiplier = 1.00m;

    private static readonly HashSet<(EnergyType Attacker, EnergyType Defender)> Advantages =
    [
        (EnergyType.NaturalEnergy, EnergyType.Aura),
        (EnergyType.AbyssalEnergy, EnergyType.Aura),
        (EnergyType.NaturalEnergy, EnergyType.Mana),
        (EnergyType.AbyssalEnergy, EnergyType.Prana),
        (EnergyType.NaturalEnergy, EnergyType.Prana),
        (EnergyType.AbyssalEnergy, EnergyType.Mana),
        (EnergyType.Aura, EnergyType.Mana),
        (EnergyType.Prana, EnergyType.Aura),
        (EnergyType.Mana, EnergyType.Prana),
        (EnergyType.AstralPower, EnergyType.Aura),
        (EnergyType.AstralPower, EnergyType.Mana),
        (EnergyType.AstralPower, EnergyType.Prana)
    ];

    private static readonly HashSet<(EnergyType First, EnergyType Second)> ReciprocalPairs =
    [
        (EnergyType.NaturalEnergy, EnergyType.AbyssalEnergy),
        (EnergyType.NaturalEnergy, EnergyType.AstralPower),
        (EnergyType.AbyssalEnergy, EnergyType.AstralPower)
    ];

    public EnergyAffinityRelation GetRelation(EnergyType attacker, EnergyType defender)
    {
        Validate(attacker, nameof(attacker));
        Validate(defender, nameof(defender));

        if (attacker == defender)
            return EnergyAffinityRelation.Neutral;

        if (IsReciprocalPair(attacker, defender))
            return EnergyAffinityRelation.Reciprocal;

        if (Advantages.Contains((attacker, defender)))
            return EnergyAffinityRelation.Advantage;

        if (Advantages.Contains((defender, attacker)))
            return EnergyAffinityRelation.Disadvantage;

        return EnergyAffinityRelation.Neutral;
    }

    public decimal GetMultiplier(EnergyType attacker, EnergyType defender) =>
        GetRelation(attacker, defender) switch
        {
            EnergyAffinityRelation.Advantage => AdvantageMultiplier,
            EnergyAffinityRelation.Disadvantage => DisadvantageMultiplier,
            EnergyAffinityRelation.Reciprocal => ReciprocalMultiplier,
            EnergyAffinityRelation.Neutral => NeutralMultiplier,
            _ => throw new InvalidOperationException("Unknown energy affinity relation.")
        };

    private static bool IsReciprocalPair(EnergyType first, EnergyType second) =>
        ReciprocalPairs.Contains((first, second)) || ReciprocalPairs.Contains((second, first));

    private static void Validate(EnergyType value, string parameterName)
    {
        if (!Enum.IsDefined(value))
            throw new ArgumentOutOfRangeException(parameterName, value, "Unknown energy type.");
    }
}

/// <summary>Applies the affinity matrix once to a compatible energy damage component.</summary>
public sealed class EnergyAffinityResolver
{
    private readonly EnergyAffinityMatrix _matrix;

    public EnergyAffinityResolver(EnergyAffinityMatrix? matrix = null)
    {
        _matrix = matrix ?? new EnergyAffinityMatrix();
    }

    public Result<EnergyAffinityResult> Resolve(
        EnergyType attackerEnergy,
        EnergyType defenderEnergy,
        decimal damageBeforeAffinity)
    {
        if (damageBeforeAffinity < 0m)
        {
            return Result<EnergyAffinityResult>.Failure(new GameError(
                "combat.affinity.negative_damage",
                "Damage before affinity cannot be negative."));
        }

        try
        {
            var relation = _matrix.GetRelation(attackerEnergy, defenderEnergy);
            var multiplier = _matrix.GetMultiplier(attackerEnergy, defenderEnergy);
            return Result<EnergyAffinityResult>.Success(new EnergyAffinityResult(
                attackerEnergy,
                defenderEnergy,
                relation,
                multiplier,
                damageBeforeAffinity,
                damageBeforeAffinity * multiplier));
        }
        catch (ArgumentOutOfRangeException)
        {
            return Result<EnergyAffinityResult>.Failure(new GameError(
                "combat.affinity.invalid_energy",
                "The attacker and defender must use defined energy types."));
        }
    }
}
