namespace MythicalExodus.Domain.Statistics;

/// <summary>Stable, ordinal identifier for a domain statistic.</summary>
public readonly record struct StatId
{
    public StatId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
    }

    public string Value { get; }
    public bool IsValid => !string.IsNullOrWhiteSpace(Value);

    public override string ToString() => Value ?? string.Empty;
}
