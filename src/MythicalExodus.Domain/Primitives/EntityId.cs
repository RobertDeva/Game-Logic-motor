namespace MythicalExodus.Domain.Primitives;

/// <summary>Stable identity for an entity within a game session or persisted world.</summary>
public readonly record struct EntityId(Guid Value)
{
    public static EntityId New() => new(Guid.NewGuid());
    public bool IsEmpty => Value == Guid.Empty;
    public override string ToString() => Value.ToString("N");
}
