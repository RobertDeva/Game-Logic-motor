namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Already-resolved incoming changes to the two independent combat resource
/// channels. This value contains magnitudes only; it does not calculate damage.
/// </summary>
public sealed record CombatResourceImpact
{
    public CombatResourceImpact(decimal vitalityDamage, decimal posturePressure)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityDamage);
        ArgumentOutOfRangeException.ThrowIfNegative(posturePressure);

        VitalityDamage = vitalityDamage;
        PosturePressure = posturePressure;
    }

    public decimal VitalityDamage { get; }
    public decimal PosturePressure { get; }
}
