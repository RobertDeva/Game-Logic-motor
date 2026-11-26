namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Characters;
using MythicalExodus.Domain.Statistics;

public readonly record struct DamageTypeId
{
    public DamageTypeId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
    }

    public string Value { get; }
    public bool IsValid => !string.IsNullOrWhiteSpace(Value);
    public override string ToString() => Value ?? string.Empty;
}

/// <summary>Damage component after offense has been calculated and before target defenses.</summary>
public sealed record IncomingDamageComponent
{
    public IncomingDamageComponent(string componentKey, DamageTypeId damageType, decimal offensivePower)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(componentKey);
        if (!damageType.IsValid)
            throw new ArgumentException("A valid damage type is required.", nameof(damageType));
        ArgumentOutOfRangeException.ThrowIfNegative(offensivePower);
        ComponentKey = componentKey.Trim();
        DamageType = damageType;
        OffensivePower = offensivePower;
    }

    public string ComponentKey { get; }
    public DamageTypeId DamageType { get; }
    public decimal OffensivePower { get; }
}

/// <summary>
/// Target defenses for one damage type. The stat-derived resistance cap is
/// constrained to the v1.0 maximum of 20%; total resistance limits are content policy.
/// </summary>
public sealed record DamageResistanceProfile
{
    public const decimal MaximumStatResistanceContribution = 0.20m;

    public DamageResistanceProfile(
        DamageTypeId damageType,
        decimal flatDefense,
        decimal statResistanceCap,
        decimal equipmentResistance,
        decimal stateResistance,
        decimal minimumTotalResistance,
        decimal maximumTotalResistance,
        decimal vulnerabilityMultiplier,
        bool isImmune = false)
    {
        if (!damageType.IsValid)
            throw new ArgumentException("A valid damage type is required.", nameof(damageType));
        ArgumentOutOfRangeException.ThrowIfNegative(flatDefense);
        ArgumentOutOfRangeException.ThrowIfNegative(statResistanceCap);
        if (statResistanceCap > MaximumStatResistanceContribution)
            throw new ArgumentOutOfRangeException(nameof(statResistanceCap),
                "The stat-derived resistance contribution cannot exceed 20% in model v1.0.");
        if (minimumTotalResistance < 0m || minimumTotalResistance > 1m)
            throw new ArgumentOutOfRangeException(nameof(minimumTotalResistance));
        if (maximumTotalResistance < minimumTotalResistance || maximumTotalResistance > 1m)
            throw new ArgumentOutOfRangeException(nameof(maximumTotalResistance));
        ArgumentOutOfRangeException.ThrowIfNegative(vulnerabilityMultiplier);

        DamageType = damageType;
        FlatDefense = flatDefense;
        StatResistanceCap = statResistanceCap;
        EquipmentResistance = equipmentResistance;
        StateResistance = stateResistance;
        MinimumTotalResistance = minimumTotalResistance;
        MaximumTotalResistance = maximumTotalResistance;
        VulnerabilityMultiplier = vulnerabilityMultiplier;
        IsImmune = isImmune;
    }

    public DamageTypeId DamageType { get; }
    public decimal FlatDefense { get; }
    public decimal StatResistanceCap { get; }
    public decimal EquipmentResistance { get; }
    public decimal StateResistance { get; }
    public decimal MinimumTotalResistance { get; }
    public decimal MaximumTotalResistance { get; }
    public decimal VulnerabilityMultiplier { get; }
    public bool IsImmune { get; }
}

public sealed record DefensiveDamageComponentResult(
    string ComponentKey,
    DamageTypeId DamageType,
    decimal OffensivePower,
    decimal FlatDefense,
    decimal PowerAfterFlatDefense,
    decimal StatResistance,
    decimal EquipmentResistance,
    decimal StateResistance,
    decimal TotalResistance,
    decimal VulnerabilityMultiplier,
    bool IsImmune,
    decimal FinalDamage);

public sealed class DefensiveDamageCalculationResult
{
    private readonly DefensiveDamageComponentResult[] _components;
    private readonly IReadOnlyList<DefensiveDamageComponentResult> _readOnlyComponents;

    public DefensiveDamageCalculationResult(IEnumerable<DefensiveDamageComponentResult> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = components.ToArray();
        if (_components.Length == 0 || _components.Any(component => component is null))
            throw new ArgumentException("At least one defensive damage component is required.", nameof(components));
        _readOnlyComponents = Array.AsReadOnly(_components);
        TotalDamage = _components.Sum(component => component.FinalDamage);
    }

    public IReadOnlyList<DefensiveDamageComponentResult> Components => _readOnlyComponents;
    public decimal TotalDamage { get; }
}

/// <summary>Applies flat defense, capped resistance, immunity and vulnerability to each component.</summary>
public sealed class DefensiveDamageCalculator
{
    private readonly AttributeScalingCurveSet _curves;

    public DefensiveDamageCalculator(AttributeScalingCurveSet? curves = null)
    {
        _curves = curves ?? AttributeScalingCurveSet.Version1;
    }

    public Result<DefensiveDamageCalculationResult> Calculate(
        CharacterAttributes defenderAttributes,
        IEnumerable<IncomingDamageComponent> incomingComponents,
        IReadOnlyDictionary<DamageTypeId, DamageResistanceProfile> resistanceProfiles)
    {
        ArgumentNullException.ThrowIfNull(defenderAttributes);
        ArgumentNullException.ThrowIfNull(incomingComponents);
        ArgumentNullException.ThrowIfNull(resistanceProfiles);

        var incoming = incomingComponents.ToArray();
        if (incoming.Length == 0)
        {
            return Failure("combat.defense.no_components", "A defensive calculation requires at least one incoming component.");
        }
        if (incoming.Any(component => component is null))
        {
            return Failure("combat.defense.null_component", "Incoming damage components cannot contain null entries.");
        }
        if (incoming.Select(component => component.ComponentKey).Distinct(StringComparer.Ordinal).Count() != incoming.Length)
        {
            return Failure("combat.defense.duplicate_component_key", "Incoming component keys must be unique.");
        }

        var results = new List<DefensiveDamageComponentResult>(incoming.Length);
        var resistanceCurve = _curves.Evaluate(
            CharacterAttribute.Endurance,
            defenderAttributes.Endurance);

        foreach (var component in incoming)
        {
            if (!resistanceProfiles.TryGetValue(component.DamageType, out var profile) || profile is null)
            {
                return Failure(
                    "combat.defense.missing_resistance_profile",
                    $"No resistance profile is configured for damage type '{component.DamageType}'.");
            }
            if (profile.DamageType != component.DamageType)
            {
                return Failure(
                    "combat.defense.resistance_profile_type_mismatch",
                    "The resistance profile key does not match its damage type.");
            }

            var statResistance = profile.StatResistanceCap * resistanceCurve;
            var totalResistance = Math.Clamp(
                statResistance + profile.EquipmentResistance + profile.StateResistance,
                profile.MinimumTotalResistance,
                profile.MaximumTotalResistance);
            var powerAfterFlatDefense = Math.Max(0m, component.OffensivePower - profile.FlatDefense);
            var finalDamage = profile.IsImmune
                ? 0m
                : powerAfterFlatDefense * (1m - totalResistance) * profile.VulnerabilityMultiplier;

            results.Add(new DefensiveDamageComponentResult(
                component.ComponentKey,
                component.DamageType,
                component.OffensivePower,
                profile.FlatDefense,
                powerAfterFlatDefense,
                statResistance,
                profile.EquipmentResistance,
                profile.StateResistance,
                totalResistance,
                profile.VulnerabilityMultiplier,
                profile.IsImmune,
                finalDamage));
        }

        return Result<DefensiveDamageCalculationResult>.Success(new DefensiveDamageCalculationResult(results));
    }

    private static Result<DefensiveDamageCalculationResult> Failure(string code, string message) =>
        Result<DefensiveDamageCalculationResult>.Failure(new GameError(code, message));
}
