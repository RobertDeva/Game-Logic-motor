namespace MythicalExodus.Domain.Statistics;

using MythicalExodus.Domain.Characters;

/// <summary>
/// Piecewise-linear normalized attribute contribution curve. Weights represent
/// marginal value per point in each segment; the curve is continuous on 1..99.
/// </summary>
public sealed record AttributeScalingCurve
{
    public AttributeScalingCurve(decimal firstWeight, decimal secondWeight, decimal thirdWeight, decimal fourthWeight)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(firstWeight);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(secondWeight);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(thirdWeight);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(fourthWeight);

        FirstWeight = firstWeight;
        SecondWeight = secondWeight;
        ThirdWeight = thirdWeight;
        FourthWeight = fourthWeight;
        Denominator = (24m * firstWeight) + (25m * secondWeight) +
                      (25m * thirdWeight) + (24m * fourthWeight);
    }

    public decimal FirstWeight { get; }
    public decimal SecondWeight { get; }
    public decimal ThirdWeight { get; }
    public decimal FourthWeight { get; }
    private decimal Denominator { get; }

    public decimal Evaluate(int attributeValue)
    {
        if (attributeValue is < CharacterAttributes.MinimumValue or > CharacterAttributes.MaximumValue)
            throw new ArgumentOutOfRangeException(nameof(attributeValue));

        var firstSegment = Math.Clamp(attributeValue - 1, 0, 24);
        var secondSegment = Math.Clamp(attributeValue - 25, 0, 25);
        var thirdSegment = Math.Clamp(attributeValue - 50, 0, 25);
        var fourthSegment = Math.Clamp(attributeValue - 75, 0, 24);

        return (
            (FirstWeight * firstSegment) +
            (SecondWeight * secondSegment) +
            (ThirdWeight * thirdSegment) +
            (FourthWeight * fourthSegment)) / Denominator;
    }
}

/// <summary>Versioned attribute curve data from the combat mathematical model v1.0.</summary>
public sealed class AttributeScalingCurveSet
{
    private readonly IReadOnlyDictionary<CharacterAttribute, AttributeScalingCurve> _curves;

    public AttributeScalingCurveSet(IReadOnlyDictionary<CharacterAttribute, AttributeScalingCurve> curves)
    {
        ArgumentNullException.ThrowIfNull(curves);
        var copy = new Dictionary<CharacterAttribute, AttributeScalingCurve>();
        foreach (var attribute in Enum.GetValues<CharacterAttribute>())
        {
            if (!curves.TryGetValue(attribute, out var curve) || curve is null)
                throw new ArgumentException($"A scaling curve is required for {attribute}.", nameof(curves));
            copy.Add(attribute, curve);
        }

        if (curves.Keys.Any(attribute => !Enum.IsDefined(attribute)))
            throw new ArgumentException("The curve set contains an unknown character attribute.", nameof(curves));

        _curves = new System.Collections.ObjectModel.ReadOnlyDictionary<CharacterAttribute, AttributeScalingCurve>(copy);
    }

    public decimal Evaluate(CharacterAttribute attribute, int value)
    {
        if (!_curves.TryGetValue(attribute, out var curve))
            throw new ArgumentOutOfRangeException(nameof(attribute), attribute, "Unknown character attribute.");
        return curve.Evaluate(value);
    }

    public static AttributeScalingCurveSet Version1 { get; } = new(
        new Dictionary<CharacterAttribute, AttributeScalingCurve>
        {
            [CharacterAttribute.Vitality] = new(1.25m, 1.00m, 0.55m, 0.25m),
            [CharacterAttribute.Strength] = new(0.70m, 1.10m, 0.80m, 0.35m),
            [CharacterAttribute.Agility] = new(1.10m, 0.85m, 0.45m, 0.20m),
            [CharacterAttribute.Endurance] = new(1.20m, 0.90m, 0.45m, 0.20m),
            [CharacterAttribute.Willpower] = new(0.65m, 0.95m, 0.85m, 0.45m),
            [CharacterAttribute.Rupture] = new(0.55m, 0.95m, 0.75m, 0.35m),
            [CharacterAttribute.Dexterity] = new(0.75m, 1.10m, 0.75m, 0.30m),
            [CharacterAttribute.Harmony] = new(0.65m, 0.95m, 0.90m, 0.40m)
        });
}
