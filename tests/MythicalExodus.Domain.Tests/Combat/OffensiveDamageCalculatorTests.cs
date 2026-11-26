using MythicalExodus.Domain.Characters;
using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Statistics;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class OffensiveDamageCalculatorTests
{
    private readonly OffensiveDamageCalculator _calculator = new();

    [Fact]
    public void Every_version_one_curve_has_zero_and_one_endpoints_and_is_monotonic()
    {
        foreach (var attribute in Enum.GetValues<CharacterAttribute>())
        {
            var previous = AttributeScalingCurveSet.Version1.Evaluate(attribute, 1);
            Assert.Equal(0m, previous);
            for (var value = 2; value <= 99; value++)
            {
                var current = AttributeScalingCurveSet.Version1.Evaluate(attribute, value);
                Assert.InRange(current, 0m, 1m);
                Assert.True(current > previous, $"{attribute} curve did not increase at {value}.");
                previous = current;
            }
            Assert.Equal(1m, previous);
        }
    }

    [Fact]
    public void Strength_scaling_uses_the_component_coefficient_and_base_power()
    {
        var attributes = new CharacterAttributes(1, 99, 1, 1, 1, 1, 1, 1);
        var component = new OffensiveDamageComponentDefinition(
            "physical", 100m, new[] { new AttributeScalingCoefficient(CharacterAttribute.Strength, 0.75m) });

        var result = _calculator.Calculate(attributes, new[] { component });

        Assert.True(result.IsSuccess);
        Assert.Equal(1.75m, result.Value.Components[0].AttributeScalingFactor);
        Assert.Equal(175m, result.Value.Components[0].AttackRating);
        Assert.Equal(175m, result.Value.TotalPower);
    }

    [Fact]
    public void Willpower_adds_up_to_fifteen_percent_to_every_damage_component_once()
    {
        var attributes = new CharacterAttributes(1, 1, 1, 1, 99, 1, 1, 1);
        var component = new OffensiveDamageComponentDefinition("energy", 100m, Array.Empty<AttributeScalingCoefficient>());

        var result = _calculator.Calculate(attributes, new[] { component });

        Assert.True(result.IsSuccess);
        Assert.Equal(1.15m, result.Value.Components[0].WillpowerMultiplier);
        Assert.Equal(115m, result.Value.TotalPower);
    }

    [Fact]
    public void Movement_ability_and_condition_multipliers_are_applied_to_attack_rating()
    {
        var attributes = new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1);
        var component = new OffensiveDamageComponentDefinition(
            "weapon", 100m, Array.Empty<AttributeScalingCoefficient>(),
            movementMultiplier: 1.2m, abilityMultiplier: 1.5m, conditionMultiplier: 0.8m);

        var result = _calculator.Calculate(attributes, new[] { component });

        Assert.True(result.IsSuccess);
        Assert.Equal(144m, result.Value.TotalPower);
    }

    [Fact]
    public void Multiple_components_are_calculated_independently_and_summed()
    {
        var attributes = new CharacterAttributes(1, 99, 1, 1, 1, 1, 1, 99);
        var components = new[]
        {
            new OffensiveDamageComponentDefinition("physical", 100m,
                new[] { new AttributeScalingCoefficient(CharacterAttribute.Strength, 0.5m) }),
            new OffensiveDamageComponentDefinition("arcane", 50m,
                new[] { new AttributeScalingCoefficient(CharacterAttribute.Harmony, 0.25m) })
        };

        var result = _calculator.Calculate(attributes, components);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Components.Count);
        Assert.Equal(150m, result.Value.Components[0].AttackRating);
        Assert.Equal(62.5m, result.Value.Components[1].AttackRating);
        Assert.Equal(212.5m, result.Value.TotalPower);
    }

    [Fact]
    public void Unspecified_attributes_do_not_scale_damage_implicitly()
    {
        var attributes = new CharacterAttributes(99, 99, 99, 99, 1, 99, 99, 99);
        var component = new OffensiveDamageComponentDefinition("unscaled", 80m, Array.Empty<AttributeScalingCoefficient>());

        var result = _calculator.Calculate(attributes, new[] { component });

        Assert.True(result.IsSuccess);
        Assert.Equal(80m, result.Value.TotalPower);
    }

    [Fact]
    public void Scaling_coefficient_budget_cannot_exceed_point_seventy_five()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new OffensiveDamageComponentDefinition("invalid", 100m, new[]
            {
                new AttributeScalingCoefficient(CharacterAttribute.Strength, 0.5m),
                new AttributeScalingCoefficient(CharacterAttribute.Dexterity, 0.26m)
            }));
    }

    [Fact]
    public void Non_offensive_attributes_cannot_be_used_as_direct_damage_scaling_coefficients()
    {
        Assert.Throws<ArgumentException>(() =>
            new OffensiveDamageComponentDefinition("invalid", 100m, new[]
            {
                new AttributeScalingCoefficient(CharacterAttribute.Willpower, 0.1m)
            }));
        Assert.Throws<ArgumentException>(() =>
            new OffensiveDamageComponentDefinition("invalid", 100m, new[]
            {
                new AttributeScalingCoefficient(CharacterAttribute.Agility, 0.1m)
            }));
    }

    [Fact]
    public void Component_cannot_declare_an_attribute_twice()
    {
        Assert.Throws<ArgumentException>(() =>
            new OffensiveDamageComponentDefinition("invalid", 100m, new[]
            {
                new AttributeScalingCoefficient(CharacterAttribute.Strength, 0.2m),
                new AttributeScalingCoefficient(CharacterAttribute.Strength, 0.3m)
            }));
    }

    [Fact]
    public void Duplicate_component_keys_are_rejected_as_a_typed_error()
    {
        var componentA = new OffensiveDamageComponentDefinition("physical", 10m, Array.Empty<AttributeScalingCoefficient>());
        var componentB = new OffensiveDamageComponentDefinition("physical", 20m, Array.Empty<AttributeScalingCoefficient>());

        var result = _calculator.Calculate(new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1), new[] { componentA, componentB });

        Assert.True(result.IsFailure);
        Assert.Equal("combat.offense.duplicate_component_key", result.Error!.Code);
    }

    [Fact]
    public void Empty_component_collection_is_rejected()
    {
        var result = _calculator.Calculate(new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1), Array.Empty<OffensiveDamageComponentDefinition>());
        Assert.True(result.IsFailure);
        Assert.Equal("combat.offense.no_components", result.Error!.Code);
    }
}
