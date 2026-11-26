namespace MythicalExodus.Domain.Characters;

/// <summary>Authoritative initial primary attributes for each player class.</summary>
public static class PlayerClassDefaults
{
    public static CharacterAttributes GetInitialAttributes(PlayerClass playerClass) =>
        playerClass switch
        {
            PlayerClass.MagicSwordsman => new CharacterAttributes(
                vitality: 5, strength: 6, agility: 5, endurance: 4,
                willpower: 4, rupture: 5, dexterity: 6, harmony: 6),

            PlayerClass.Warrior => new CharacterAttributes(
                vitality: 6, strength: 6, agility: 4, endurance: 6,
                willpower: 5, rupture: 5, dexterity: 4, harmony: 4),

            PlayerClass.Reaper => new CharacterAttributes(
                vitality: 4, strength: 4, agility: 6, endurance: 4,
                willpower: 6, rupture: 6, dexterity: 5, harmony: 5),

            _ => throw new ArgumentOutOfRangeException(
                nameof(playerClass), playerClass, "Unknown player class.")
        };
}
