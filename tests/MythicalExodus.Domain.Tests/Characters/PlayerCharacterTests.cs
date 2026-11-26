using MythicalExodus.Domain.Characters;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Characters;

public sealed class PlayerCharacterTests
{
    [Theory]
    [InlineData(PlayerClass.MagicSwordsman, 5, 6, 5, 4, 4, 5, 6, 6, 41)]
    [InlineData(PlayerClass.Warrior, 6, 6, 4, 6, 5, 5, 4, 4, 40)]
    [InlineData(PlayerClass.Reaper, 4, 4, 6, 4, 6, 6, 5, 5, 40)]
    public void New_character_uses_class_starting_attributes_and_level_one(
        PlayerClass playerClass,
        int vitality,
        int strength,
        int agility,
        int endurance,
        int willpower,
        int rupture,
        int dexterity,
        int harmony,
        int expectedTotal)
    {
        var character = Create(playerClass);

        Assert.Equal(new CharacterAttributes(
            vitality, strength, agility, endurance,
            willpower, rupture, dexterity, harmony), character.Attributes);
        Assert.Equal(expectedTotal, character.Attributes.Total);
        Assert.Equal(1, character.Level);
    }

    [Theory]
    [InlineData(PlayerClass.MagicSwordsman)]
    [InlineData(PlayerClass.Warrior)]
    [InlineData(PlayerClass.Reaper)]
    public void Increasing_one_attribute_increases_level_by_exactly_one(PlayerClass playerClass)
    {
        var character = Create(playerClass);
        var previousAttributes = character.Attributes;

        var result = character.TryIncreaseAttribute(CharacterAttribute.Vitality);

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value);
        Assert.Equal(previousAttributes.Vitality + 1, character.Attributes.Vitality);
        Assert.Equal(previousAttributes.Total + 1, character.Attributes.Total);
        Assert.Equal(2, character.Level);
    }

    [Fact]
    public void Attribute_at_cap_is_rejected_without_changing_character()
    {
        var initial = PlayerClassDefaults.GetInitialAttributes(PlayerClass.Warrior);
        var capped = new CharacterAttributes(
            99, initial.Strength, initial.Agility, initial.Endurance,
            initial.Willpower, initial.Rupture, initial.Dexterity, initial.Harmony);
        var character = new PlayerCharacter(EntityId.New(), PlayerClass.Warrior, capped);
        var before = character.Attributes;
        var previousLevel = character.Level;

        var result = character.TryIncreaseAttribute(CharacterAttribute.Vitality);

        Assert.True(result.IsFailure);
        Assert.Equal("character.attribute.maximum_reached", result.Error!.Code);
        Assert.Equal(before, character.Attributes);
        Assert.Equal(previousLevel, character.Level);
    }

    [Fact]
    public void Attribute_below_class_baseline_is_rejected()
    {
        var attributes = new CharacterAttributes(4, 6, 5, 4, 4, 5, 6, 6);

        Assert.Throws<ArgumentException>(() =>
            new PlayerCharacter(EntityId.New(), PlayerClass.MagicSwordsman, attributes));
    }

    [Fact]
    public void Unknown_attribute_is_rejected_without_changing_character()
    {
        var character = Create(PlayerClass.Reaper);
        var before = character.Attributes;

        var result = character.TryIncreaseAttribute((CharacterAttribute)999);

        Assert.True(result.IsFailure);
        Assert.Equal("character.attribute.unknown", result.Error!.Code);
        Assert.Equal(before, character.Attributes);
    }

    [Fact]
    public void Player_class_is_fixed_after_creation()
    {
        var character = Create(PlayerClass.Warrior);

        Assert.Equal(PlayerClass.Warrior, character.Class);
    }

    private static PlayerCharacter Create(PlayerClass playerClass) =>
        new(EntityId.New(), playerClass, PlayerClassDefaults.GetInitialAttributes(playerClass));
}
