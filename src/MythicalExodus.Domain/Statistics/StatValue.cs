namespace MythicalExodus.Domain.Statistics;

/// <summary>
/// Immutable base/effective value pair. This type stores values but does not decide
/// how modifiers are combined or how a statistic is derived.
/// </summary>
public sealed record StatValue
{
    public StatValue(StatId id, decimal baseValue, decimal effectiveValue)
    {
        if (!id.IsValid)
            throw new ArgumentException("A valid statistic identifier is required.", nameof(id));

        Id = id;
        BaseValue = baseValue;
        EffectiveValue = effectiveValue;
    }

    public StatId Id { get; }
    public decimal BaseValue { get; }
    public decimal EffectiveValue { get; }
}
