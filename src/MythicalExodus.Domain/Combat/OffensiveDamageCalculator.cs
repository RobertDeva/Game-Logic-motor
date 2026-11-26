namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Characters;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Statistics;

public sealed record AttributeScalingCoefficient
{
    public AttributeScalingCoefficient(CharacterAttribute attribute, decimal coefficient)
    {
        if (!Enum.IsDefined(attribute))
            throw new ArgumentOutOfRangeException(nameof(attribute));
        ArgumentOutOfRangeException.ThrowIfNegative(coefficient);
        Attribute = attribute;
        Coefficient = coefficient;
    }

    public CharacterAttribute Attribute { get; }
    public decimal Coefficient { get; }
}

/// <summary>Content-defined offensive component; contains no target defenses or affinity rules.</summary>
public sealed class OffensiveDamageComponentDefinition
{
    private readonly AttributeScalingCoefficient[] _scaling;
    private readonly IReadOnlyList<AttributeScalingCoefficient> _readOnlyScaling;

    public OffensiveDamageComponentDefinition(
        string componentKey,
        decimal basePower,
        IEnumerable<AttributeScalingCoefficient> scaling,
        decimal movementMultiplier = 1m,
        decimal abilityMultiplier = 1m,
        decimal conditionMultiplier = 1m)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(componentKey);
        ArgumentOutOfRangeException.ThrowIfNegative(basePower);
        ArgumentOutOfRangeException.ThrowIfNegative(movementMultiplier);
        ArgumentOutOfRangeException.ThrowIfNegative(abilityMultiplier);
        ArgumentOutOfRangeException.ThrowIfNegative(conditionMultiplier);
        ArgumentNullException.ThrowIfNull(scaling);

        _scaling = scaling.ToArray();
        if (_scaling.Any(item => item is null))
            throw new ArgumentException("Scaling entries cannot be null.", nameof(scaling));
        if (_scaling.Select(item => item.Attribute).Distinct().Count() != _scaling.Length)
            throw new ArgumentException("A component cannot declare the same attribute more than once.", nameof(scaling));
        if (_scaling.Any(item => item.Attribute is not (
            CharacterAttribute.Strength or CharacterAttribute.Dexterity or CharacterAttribute.Harmony)))
            throw new ArgumentException(
                "Only Strength, Dexterity, and Harmony can contribute to direct damage scaling in model v1.0. Willpower is applied separately.",
                nameof(scaling));
        if (_scaling.Sum(item => item.Coefficient) > MaximumScalingCoefficientTotal)
            throw new ArgumentOutOfRangeException(nameof(scaling),
                $"The total attribute scaling coefficient cannot exceed {MaximumScalingCoefficientTotal}.");

        ComponentKey = componentKey.Trim();
        BasePower = basePower;
        MovementMultiplier = movementMultiplier;
        AbilityMultiplier = abilityMultiplier;
        ConditionMultiplier = conditionMultiplier;
        _readOnlyScaling = Array.AsReadOnly(_scaling);
    }

    public const decimal MaximumScalingCoefficientTotal = 0.75m;
    public string ComponentKey { get; }
    public decimal BasePower { get; }
    public IReadOnlyList<AttributeScalingCoefficient> Scaling => _readOnlyScaling;
    public decimal MovementMultiplier { get; }
    public decimal AbilityMultiplier { get; }
    public decimal ConditionMultiplier { get; }
}

/// <summary>One component of the calculated offensive output, before target defense.</summary>
public sealed record OffensiveDamageComponentResult(
    string ComponentKey,
    decimal BasePower,
    decimal AttributeScalingFactor,
    decimal AttackRating,
    decimal WillpowerMultiplier,
    decimal MovementMultiplier,
    decimal AbilityMultiplier,
    decimal ConditionMultiplier,
    decimal FinalPower);

public sealed class OffensiveDamageCalculationResult
{
    private readonly OffensiveDamageComponentResult[] _components;
    private readonly IReadOnlyList<OffensiveDamageComponentResult> _readOnlyComponents;

    public OffensiveDamageCalculationResult(IEnumerable<OffensiveDamageComponentResult> components)
    {
        ArgumentNullException.ThrowIfNull(components);
        _components = components.ToArray();
        if (_components.Length == 0 || _components.Any(component => component is null))
            throw new ArgumentException("At least one calculated damage component is required.", nameof(components));
        _readOnlyComponents = Array.AsReadOnly(_components);
        TotalPower = _components.Sum(component => component.FinalPower);
    }

    public IReadOnlyList<OffensiveDamageComponentResult> Components => _readOnlyComponents;
    public decimal TotalPower { get; }
}

/// <summary>
/// Resolves base power, declared attribute scaling, action multipliers and
/// universal Willpower damage bonus. Does not resolve defense, affinity or critical hits.
/// </summary>
public sealed class OffensiveDamageCalculator
{
    public const decimal MaximumWillpowerDamageBonus = 0.15m;

    private readonly AttributeScalingCurveSet _curves;

    public OffensiveDamageCalculator(AttributeScalingCurveSet? curves = null)
    {
        _curves = curves ?? AttributeScalingCurveSet.Version1;
    }

    public Result<OffensiveDamageCalculationResult> Calculate(
        CharacterAttributes attributes,
        IEnumerable<OffensiveDamageComponentDefinition> components)
    {
        ArgumentNullException.ThrowIfNull(attributes);
        ArgumentNullException.ThrowIfNull(components);
        var definitions = components.ToArray();
        if (definitions.Length == 0)
        {
            return Result<OffensiveDamageCalculationResult>.Failure(new GameError(
                "combat.offense.no_components",
                "An offensive calculation requires at least one damage component."));
        }
        if (definitions.Any(component => component is null))
        {
            return Result<OffensiveDamageCalculationResult>.Failure(new GameError(
                "combat.offense.null_component",
                "Offensive damage components cannot contain null entries."));
        }
        if (definitions.Select(component => component.ComponentKey).Distinct(StringComparer.Ordinal).Count() != definitions.Length)
        {
            return Result<OffensiveDamageCalculationResult>.Failure(new GameError(
                "combat.offense.duplicate_component_key",
                "Offensive component keys must be unique within a calculation."));
        }

        var willpowerCurve = _curves.Evaluate(CharacterAttribute.Willpower, attributes.Willpower);
        var willpowerMultiplier = 1m + (MaximumWillpowerDamageBonus * willpowerCurve);
        var results = new List<OffensiveDamageComponentResult>(definitions.Length);

        foreach (var definition in definitions)
        {
            var scalingContribution = definition.Scaling.Sum(entry =>
                entry.Coefficient * _curves.Evaluate(entry.Attribute, attributes[entry.Attribute]));
            var scalingFactor = 1m + scalingContribution;
            var attackRating = definition.BasePower * scalingFactor;
            var finalPower = attackRating *
                             definition.MovementMultiplier *
                             definition.AbilityMultiplier *
                             definition.ConditionMultiplier *
                             willpowerMultiplier;

            results.Add(new OffensiveDamageComponentResult(
                definition.ComponentKey,
                definition.BasePower,
                scalingFactor,
                attackRating,
                willpowerMultiplier,
                definition.MovementMultiplier,
                definition.AbilityMultiplier,
                definition.ConditionMultiplier,
                finalPower));
        }

        return Result<OffensiveDamageCalculationResult>.Success(new OffensiveDamageCalculationResult(results));
    }
}
