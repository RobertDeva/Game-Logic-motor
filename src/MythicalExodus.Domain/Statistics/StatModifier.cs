using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Statistics;

/// <summary>
/// Immutable description of a modifier. It contains no activation, duration,
/// stacking, expiration, or calculation behavior.
/// </summary>
public sealed record StatModifier
{
    public StatModifier(
        EntityId id,
        EntityId sourceId,
        StatId target,
        StatModifierOperation operation,
        decimal value)
    {
        if (id.IsEmpty)
            throw new ArgumentException("A modifier identifier cannot be empty.", nameof(id));
        if (sourceId.IsEmpty)
            throw new ArgumentException("A modifier source identifier cannot be empty.", nameof(sourceId));
        if (!target.IsValid)
            throw new ArgumentException("A valid target statistic is required.", nameof(target));
        if (!Enum.IsDefined(operation))
            throw new ArgumentOutOfRangeException(nameof(operation), operation, "Unknown modifier operation.");

        Id = id;
        SourceId = sourceId;
        Target = target;
        Operation = operation;
        Value = value;
    }

    public EntityId Id { get; }
    public EntityId SourceId { get; }
    public StatId Target { get; }
    public StatModifierOperation Operation { get; }
    public decimal Value { get; }
}
