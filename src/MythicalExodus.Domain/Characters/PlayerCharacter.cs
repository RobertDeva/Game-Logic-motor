using MythicalExodus.Domain.Entities;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Characters;

/// <summary>
/// Player-owned character state. Class identity is permanent; level is derived
/// from permanent attribute increases relative to that class's starting values.
/// </summary>
public sealed class PlayerCharacter : DomainEntity
{
    public PlayerCharacter(EntityId id, PlayerClass playerClass, CharacterAttributes attributes)
        : base(id)
    {
        if (!Enum.IsDefined(playerClass))
            throw new ArgumentOutOfRangeException(nameof(playerClass), playerClass, "Unknown player class.");

        ArgumentNullException.ThrowIfNull(attributes);

        var initialAttributes = PlayerClassDefaults.GetInitialAttributes(playerClass);
        foreach (var attribute in Enum.GetValues<CharacterAttribute>())
        {
            if (attributes[attribute] < initialAttributes[attribute])
            {
                throw new ArgumentException(
                    $"The {attribute} attribute cannot be below the starting value for {playerClass}.",
                    nameof(attributes));
            }
        }

        Class = playerClass;
        InitialAttributes = initialAttributes;
        Attributes = attributes;
    }

    public PlayerClass Class { get; }
    public CharacterAttributes InitialAttributes { get; }
    public CharacterAttributes Attributes { get; private set; }

    /// <summary>
    /// Level 1 plus all permanent attribute increases from the class baseline.
    /// This does not include temporary modifiers.
    /// </summary>
    public int Level => 1 + Attributes.Total - InitialAttributes.Total;

    /// <summary>
    /// Applies one permanent attribute point after the application layer has
    /// validated and committed the corresponding soul cost.
    /// </summary>
    public Result<int> TryIncreaseAttribute(CharacterAttribute attribute)
    {
        if (!Enum.IsDefined(attribute))
        {
            return Result<int>.Failure(new GameError(
                "character.attribute.unknown",
                "The requested character attribute is not defined."));
        }

        if (Attributes[attribute] >= CharacterAttributes.MaximumValue)
        {
            return Result<int>.Failure(new GameError(
                "character.attribute.maximum_reached",
                $"The {attribute} attribute is already at its maximum value."));
        }

        Attributes = Attributes.Increase(attribute);
        return Result<int>.Success(Level);
    }
}
