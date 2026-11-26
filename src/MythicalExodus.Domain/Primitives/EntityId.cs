namespace MythicalExodus.Domain.Primitives;

/// <summary>Stable identity shared by an entity's lifetime and persisted representations.</summary>
public readonly record struct EntityId
{
    public EntityId(Guid value)
    {
        if (value == Guid.Empty)
            throw new ArgumentException("An entity identifier cannot be empty.", nameof(value));

        Value = value;
    }

    public Guid Value { get; }
    public static EntityId New() => new(Guid.NewGuid());
    public override string ToString() => Value.ToString("N");
}
