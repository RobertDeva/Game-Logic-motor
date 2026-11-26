namespace MythicalExodus.Domain.Actions;

/// <summary>Stable identifier for a domain action definition.</summary>
public readonly record struct ActionId
{
    public ActionId(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value);
        Value = value.Trim();
    }

    public string Value { get; }
    public bool IsValid => !string.IsNullOrWhiteSpace(Value);

    public override string ToString() => Value;
}
