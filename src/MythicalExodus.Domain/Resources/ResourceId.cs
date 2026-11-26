namespace MythicalExodus.Domain.Resources;

/// <summary>Stable identifier for a consumable or replenishable domain resource.</summary>
public readonly record struct ResourceId
{
    public ResourceId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
    }

    public string Value { get; }
    public bool IsValid => !string.IsNullOrWhiteSpace(Value);

    public override string ToString() => Value ?? string.Empty;
}
