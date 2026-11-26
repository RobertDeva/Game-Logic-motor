namespace MythicalExodus.Domain.Characters;

/// <summary>
/// Immutable set of primary character attributes. Each attribute is constrained to 1..99.
/// Derived combat statistics are intentionally calculated by separate domain policies.
/// </summary>
public sealed record CharacterAttributes
{
    public const int MinimumValue = 1;
    public const int MaximumValue = 99;

    public CharacterAttributes(
        int vitality,
        int strength,
        int agility,
        int endurance,
        int willpower,
        int rupture,
        int dexterity,
        int harmony)
    {
        Validate(vitality, nameof(vitality));
        Validate(strength, nameof(strength));
        Validate(agility, nameof(agility));
        Validate(endurance, nameof(endurance));
        Validate(willpower, nameof(willpower));
        Validate(rupture, nameof(rupture));
        Validate(dexterity, nameof(dexterity));
        Validate(harmony, nameof(harmony));

        Vitality = vitality;
        Strength = strength;
        Agility = agility;
        Endurance = endurance;
        Willpower = willpower;
        Rupture = rupture;
        Dexterity = dexterity;
        Harmony = harmony;
    }

    public int Vitality { get; }
    public int Strength { get; }
    public int Agility { get; }
    public int Endurance { get; }
    public int Willpower { get; }
    public int Rupture { get; }
    public int Dexterity { get; }
    public int Harmony { get; }

    public int this[CharacterAttribute attribute] => attribute switch
    {
        CharacterAttribute.Vitality => Vitality,
        CharacterAttribute.Strength => Strength,
        CharacterAttribute.Agility => Agility,
        CharacterAttribute.Endurance => Endurance,
        CharacterAttribute.Willpower => Willpower,
        CharacterAttribute.Rupture => Rupture,
        CharacterAttribute.Dexterity => Dexterity,
        CharacterAttribute.Harmony => Harmony,
        _ => throw new ArgumentOutOfRangeException(nameof(attribute), attribute, "Unknown character attribute.")
    };

    public int Total =>
        Vitality + Strength + Agility + Endurance +
        Willpower + Rupture + Dexterity + Harmony;

    public CharacterAttributes Increase(CharacterAttribute attribute, int amount = 1)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(amount);

        var current = this[attribute];
        var updated = checked(current + amount);
        Validate(updated, nameof(amount));

        return attribute switch
        {
            CharacterAttribute.Vitality => new(updated, Strength, Agility, Endurance, Willpower, Rupture, Dexterity, Harmony),
            CharacterAttribute.Strength => new(Vitality, updated, Agility, Endurance, Willpower, Rupture, Dexterity, Harmony),
            CharacterAttribute.Agility => new(Vitality, Strength, updated, Endurance, Willpower, Rupture, Dexterity, Harmony),
            CharacterAttribute.Endurance => new(Vitality, Strength, Agility, updated, Willpower, Rupture, Dexterity, Harmony),
            CharacterAttribute.Willpower => new(Vitality, Strength, Agility, Endurance, updated, Rupture, Dexterity, Harmony),
            CharacterAttribute.Rupture => new(Vitality, Strength, Agility, Endurance, Willpower, updated, Dexterity, Harmony),
            CharacterAttribute.Dexterity => new(Vitality, Strength, Agility, Endurance, Willpower, Rupture, updated, Harmony),
            CharacterAttribute.Harmony => new(Vitality, Strength, Agility, Endurance, Willpower, Rupture, Dexterity, updated),
            _ => throw new ArgumentOutOfRangeException(nameof(attribute), attribute, "Unknown character attribute.")
        };
    }

    private static void Validate(int value, string parameterName)
    {
        if (value is < MinimumValue or > MaximumValue)
            throw new ArgumentOutOfRangeException(parameterName, value,
                $"Character attributes must be between {MinimumValue} and {MaximumValue}.");
    }
}
