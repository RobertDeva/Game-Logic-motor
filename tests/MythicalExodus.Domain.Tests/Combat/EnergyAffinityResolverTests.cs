using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class EnergyAffinityResolverTests
{
    private readonly EnergyAffinityMatrix _matrix = new();
    private readonly EnergyAffinityResolver _resolver = new();

    public static TheoryData<EnergyType, EnergyType> AdvantagePairs => new()
    {
        { EnergyType.NaturalEnergy, EnergyType.Aura },
        { EnergyType.AbyssalEnergy, EnergyType.Aura },
        { EnergyType.NaturalEnergy, EnergyType.Mana },
        { EnergyType.AbyssalEnergy, EnergyType.Prana },
        { EnergyType.NaturalEnergy, EnergyType.Prana },
        { EnergyType.AbyssalEnergy, EnergyType.Mana },
        { EnergyType.Aura, EnergyType.Mana },
        { EnergyType.Prana, EnergyType.Aura },
        { EnergyType.Mana, EnergyType.Prana },
        { EnergyType.AstralPower, EnergyType.Aura },
        { EnergyType.AstralPower, EnergyType.Mana },
        { EnergyType.AstralPower, EnergyType.Prana }
    };

    public static TheoryData<EnergyType, EnergyType> ReciprocalPairs => new()
    {
        { EnergyType.NaturalEnergy, EnergyType.AbyssalEnergy },
        { EnergyType.NaturalEnergy, EnergyType.AstralPower },
        { EnergyType.AbyssalEnergy, EnergyType.AstralPower }
    };

    [Theory]
    [MemberData(nameof(AdvantagePairs))]
    public void Directed_arrow_origin_has_advantage(EnergyType attacker, EnergyType defender)
    {
        Assert.Equal(EnergyAffinityRelation.Advantage, _matrix.GetRelation(attacker, defender));
        Assert.Equal(1.50m, _matrix.GetMultiplier(attacker, defender));
    }

    [Theory]
    [MemberData(nameof(AdvantagePairs))]
    public void Reverse_of_directed_arrow_has_disadvantage(EnergyType advantage, EnergyType disadvantage)
    {
        Assert.Equal(EnergyAffinityRelation.Disadvantage, _matrix.GetRelation(disadvantage, advantage));
        Assert.Equal(0.75m, _matrix.GetMultiplier(disadvantage, advantage));
    }

    [Theory]
    [MemberData(nameof(ReciprocalPairs))]
    public void Reciprocal_pairs_use_one_point_two_five_in_both_directions(EnergyType first, EnergyType second)
    {
        Assert.Equal(EnergyAffinityRelation.Reciprocal, _matrix.GetRelation(first, second));
        Assert.Equal(EnergyAffinityRelation.Reciprocal, _matrix.GetRelation(second, first));
        Assert.Equal(1.25m, _matrix.GetMultiplier(first, second));
        Assert.Equal(1.25m, _matrix.GetMultiplier(second, first));
    }

    [Theory]
    [InlineData(EnergyType.Aura)]
    [InlineData(EnergyType.Mana)]
    [InlineData(EnergyType.Prana)]
    [InlineData(EnergyType.NaturalEnergy)]
    [InlineData(EnergyType.AbyssalEnergy)]
    [InlineData(EnergyType.AstralPower)]
    public void Same_energy_is_neutral(EnergyType energy)
    {
        Assert.Equal(EnergyAffinityRelation.Neutral, _matrix.GetRelation(energy, energy));
        Assert.Equal(1m, _matrix.GetMultiplier(energy, energy));
    }

    [Fact]
    public void Resolver_applies_affinity_to_damage_once()
    {
        var result = _resolver.Resolve(EnergyType.NaturalEnergy, EnergyType.Aura, 100m);

        Assert.True(result.IsSuccess);
        Assert.Equal(150m, result.Value.DamageAfterAffinity);
    }

    [Fact]
    public void Reciprocal_relation_overrides_ordinary_advantage_and_disadvantage()
    {
        var firstToSecond = _resolver.Resolve(EnergyType.NaturalEnergy, EnergyType.AbyssalEnergy, 100m);
        var secondToFirst = _resolver.Resolve(EnergyType.AbyssalEnergy, EnergyType.NaturalEnergy, 100m);

        Assert.Equal(125m, firstToSecond.Value.DamageAfterAffinity);
        Assert.Equal(125m, secondToFirst.Value.DamageAfterAffinity);
    }

    [Fact]
    public void Negative_damage_is_rejected()
    {
        var result = _resolver.Resolve(EnergyType.Aura, EnergyType.Mana, -1m);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.affinity.negative_damage", result.Error!.Code);
    }

    [Fact]
    public void Undefined_energy_is_rejected()
    {
        var result = _resolver.Resolve((EnergyType)999, EnergyType.Mana, 10m);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.affinity.invalid_energy", result.Error!.Code);
    }
}
