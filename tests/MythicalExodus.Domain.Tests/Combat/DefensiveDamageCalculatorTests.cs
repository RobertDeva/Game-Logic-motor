using MythicalExodus.Domain.Characters;
using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class DefensiveDamageCalculatorTests
{
    private readonly DefensiveDamageCalculator _calculator = new();
    private static readonly DamageTypeId Physical = new("physical");

    private static DamageResistanceProfile Profile(
        decimal flat = 0m,
        decimal statCap = 0m,
        decimal equipment = 0m,
        decimal state = 0m,
        decimal min = 0m,
        decimal max = 1m,
        decimal vulnerability = 1m,
        bool immune = false) =>
        new(Physical, flat, statCap, equipment, state, min, max, vulnerability, immune);

    [Fact]
    public void Applies_flat_defense_before_percentage_resistance_and_vulnerability_after()
    {
        var defender = new CharacterAttributes(1, 1, 1, 99, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 100m) };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(flat: 20m, statCap: 0.20m, max: 0.20m, vulnerability: 1.5m)
        };

        var result = _calculator.Calculate(defender, input, profiles);

        Assert.True(result.IsSuccess);
        Assert.Equal(80m, result.Value.Components[0].PowerAfterFlatDefense);
        Assert.Equal(0.20m, result.Value.Components[0].StatResistance);
        Assert.Equal(0.20m, result.Value.Components[0].TotalResistance);
        Assert.Equal(96m, result.Value.TotalDamage);
    }

    [Fact]
    public void Resistance_stat_contribution_uses_endurance_curve()
    {
        var highEndurance = new CharacterAttributes(1, 1, 1, 99, 1, 1, 1, 1);
        var lowEndurance = new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 100m) };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(statCap: 0.20m, max: 0.20m)
        };

        var highResult = _calculator.Calculate(highEndurance, input, profiles);
        var lowResult = _calculator.Calculate(lowEndurance, input, profiles);

        Assert.Equal(80m, highResult.Value.TotalDamage);
        Assert.Equal(100m, lowResult.Value.TotalDamage);
    }

    [Fact]
    public void Combined_resistance_is_clamped_to_content_defined_limits()
    {
        var defender = new CharacterAttributes(1, 1, 1, 99, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 100m) };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(statCap: 0.20m, equipment: 0.30m, state: 0.40m, min: 0.10m, max: 0.60m)
        };

        var result = _calculator.Calculate(defender, input, profiles);

        Assert.True(result.IsSuccess);
        Assert.Equal(0.60m, result.Value.Components[0].TotalResistance);
        Assert.Equal(40m, result.Value.TotalDamage);
    }

    [Fact]
    public void Resistance_reduction_is_clamped_to_minimum()
    {
        var defender = new CharacterAttributes(1, 1, 1, 99, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 100m) };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(statCap: 0.20m, state: -0.50m, min: 0.05m, max: 0.60m)
        };

        var result = _calculator.Calculate(defender, input, profiles);

        Assert.Equal(0.05m, result.Value.Components[0].TotalResistance);
        Assert.Equal(95m, result.Value.TotalDamage);
    }

    [Fact]
    public void Immunity_blocks_damage_independently_of_resistance_and_vulnerability()
    {
        var defender = new CharacterAttributes(1, 1, 1, 99, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 100m) };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(flat: 10m, statCap: 0.20m, max: 0.20m, vulnerability: 3m, immune: true)
        };

        var result = _calculator.Calculate(defender, input, profiles);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Components[0].IsImmune);
        Assert.Equal(0m, result.Value.TotalDamage);
    }

    [Fact]
    public void Flat_defense_cannot_make_damage_negative()
    {
        var defender = new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 10m) };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(flat: 50m, max: 0m)
        };

        var result = _calculator.Calculate(defender, input, profiles);

        Assert.Equal(0m, result.Value.TotalDamage);
    }

    [Fact]
    public void Damage_types_are_resolved_independently()
    {
        var fire = new DamageTypeId("fire");
        var defender = new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1);
        var input = new[]
        {
            new IncomingDamageComponent("physical-hit", Physical, 100m),
            new IncomingDamageComponent("fire-hit", fire, 50m)
        };
        var profiles = new Dictionary<DamageTypeId, DamageResistanceProfile>
        {
            [Physical] = Profile(flat: 10m, max: 0m),
            [fire] = new DamageResistanceProfile(fire, 0m, 0m, 0m, 0m, 0m, 1m, 2m)
        };

        var result = _calculator.Calculate(defender, input, profiles);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.Components.Count);
        Assert.Equal(90m, result.Value.Components[0].FinalDamage);
        Assert.Equal(100m, result.Value.Components[1].FinalDamage);
        Assert.Equal(190m, result.Value.TotalDamage);
    }

    [Fact]
    public void Rejects_stat_resistance_cap_above_twenty_percent()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            Profile(statCap: 0.21m, max: 1m));
    }

    [Fact]
    public void Missing_damage_type_profile_returns_typed_error()
    {
        var defender = new CharacterAttributes(1, 1, 1, 1, 1, 1, 1, 1);
        var input = new[] { new IncomingDamageComponent("hit", Physical, 100m) };

        var result = _calculator.Calculate(defender, input, new Dictionary<DamageTypeId, DamageResistanceProfile>());

        Assert.True(result.IsFailure);
        Assert.Equal("combat.defense.missing_resistance_profile", result.Error!.Code);
    }
}
