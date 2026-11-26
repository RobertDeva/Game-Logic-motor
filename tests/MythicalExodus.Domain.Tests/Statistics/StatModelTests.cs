using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Statistics;

namespace MythicalExodus.Domain.Tests.Statistics;

public sealed class StatModelTests
{
    [Fact]
    public void StatId_TrimsWhitespaceAndRejectsBlankValues()
    {
        Assert.Equal("maximum-vitality", new StatId(" maximum-vitality ").Value);
        Assert.Throws<ArgumentException>(() => new StatId("   "));
    }

    [Fact]
    public void StatValue_PreservesBaseAndEffectiveValuesWithoutCalculatingThem()
    {
        var value = new StatValue(new StatId("maximum-vitality"), 100m, 125m);

        Assert.Equal(100m, value.BaseValue);
        Assert.Equal(125m, value.EffectiveValue);
    }

    [Fact]
    public void StatValue_RejectsDefaultStatId()
    {
        Assert.Throws<ArgumentException>(() => new StatValue(default, 10m, 10m));
    }

    [Fact]
    public void Modifier_PreservesItsIdentitySourceTargetOperationAndValue()
    {
        var modifierId = EntityId.New();
        var sourceId = EntityId.New();
        var target = new StatId("physical-damage");

        var modifier = new StatModifier(
            modifierId,
            sourceId,
            target,
            StatModifierOperation.AdditivePercentage,
            0.15m);

        Assert.Equal(modifierId, modifier.Id);
        Assert.Equal(sourceId, modifier.SourceId);
        Assert.Equal(target, modifier.Target);
        Assert.Equal(StatModifierOperation.AdditivePercentage, modifier.Operation);
        Assert.Equal(0.15m, modifier.Value);
    }

    [Fact]
    public void Modifier_RejectsEmptyIdentitiesAndUnknownOperations()
    {
        var sourceId = EntityId.New();
        var target = new StatId("physical-damage");

        Assert.Throws<ArgumentException>(() =>
            new StatModifier(default, sourceId, target, StatModifierOperation.Flat, 1m));

        Assert.Throws<ArgumentException>(() =>
            new StatModifier(EntityId.New(), default, target, StatModifierOperation.Flat, 1m));

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new StatModifier(EntityId.New(), sourceId, target, (StatModifierOperation)999, 1m));
    }
}
