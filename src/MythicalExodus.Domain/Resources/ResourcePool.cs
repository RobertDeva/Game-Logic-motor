namespace MythicalExodus.Domain.Resources;

/// <summary>
/// Immutable bounded resource state. Consumption, restoration, regeneration,
/// costs, and overflow policies are intentionally handled by domain rules.
/// </summary>
public sealed record ResourcePool
{
    public ResourcePool(ResourceId id, decimal current, decimal maximum)
    {
        if (!id.IsValid)
            throw new ArgumentException("A valid resource identifier is required.", nameof(id));
        if (maximum < 0m)
            throw new ArgumentOutOfRangeException(nameof(maximum), maximum, "A resource maximum cannot be negative.");
        if (current < 0m || current > maximum)
            throw new ArgumentOutOfRangeException(nameof(current), current,
                "A resource current value must be between zero and its maximum.");

        Id = id;
        Current = current;
        Maximum = maximum;
    }

    public ResourceId Id { get; }
    public decimal Current { get; }
    public decimal Maximum { get; }

    public ResourcePool WithCurrent(decimal current) => new(Id, current, Maximum);

    public ResourcePool WithMaximum(decimal maximum) => new(Id, Current, maximum);
}
