using MythicalExodus.Domain.Characters;

namespace MythicalExodus.Domain.Tests.Characters;

public sealed class CharacterAttributesTests
{
    [Fact]
    public void Constructor_StoresAllAttributes()
    {
        var attributes = new CharacterAttributes(5, 6, 5, 4, 4, 5, 6, 6);

        Assert.Equal(5, attributes.Vitality);
        Assert.Equal(6, attributes.Strength);
        Assert.Equal(5, attributes.Agility);
        Assert.Equal(4, attributes.Endurance);
        Assert.Equal(4, attributes.Willpower);
        Assert.Equal(5, attributes.Rupture);
        Assert.Equal(6, attributes.Dexterity);
        Assert.Equal(6, attributes.Harmony);
        Assert.Equal(41, attributes.Total);
    }

    [Fact]
    public void Increase_ReturnsNewValueWithoutMutatingOriginal()
    {
        var original = new CharacterAttributes(5, 6, 5, 4, 4, 5, 6, 6);
        var updated = original.Increase(CharacterAttribute.Vitality);

        Assert.Equal(5, original.Vitality);
        Assert.Equal(6, updated.Vitality);
        Assert.Equal(42, updated.Total);
    }

    [Fact]
    public void Constructor_RejectsValuesOutsideAllowedRange()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CharacterAttributes(0, 6, 5, 4, 4, 5, 6, 6));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new CharacterAttributes(5, 100, 5, 4, 4, 5, 6, 6));
    }

    [Fact]
    public void Increase_RejectsExceedingMaximum()
    {
        var attributes = new CharacterAttributes(99, 6, 5, 4, 4, 5, 6, 6);

        Assert.Throws<ArgumentOutOfRangeException>(() =>
            attributes.Increase(CharacterAttribute.Vitality));
    }

    [Fact]
    public void Indexer_RejectsUnknownAttribute()
    {
        var attributes = new CharacterAttributes(5, 6, 5, 4, 4, 5, 6, 6);

        Assert.Throws<ArgumentOutOfRangeException>(() => attributes[(CharacterAttribute)999]);
    }
}
