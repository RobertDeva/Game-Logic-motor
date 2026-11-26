using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Already-resolved incoming changes to the two independent combat resource
/// channels. This value contains magnitudes only; it does not calculate damage.
/// </summary>
public sealed record CombatResourceImpact
{
    public CombatResourceImpact(decimal vitalityDamage, decimal posturePressure, EntityId? sourceEntityId = null)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityDamage);
        ArgumentOutOfRangeException.ThrowIfNegative(posturePressure);

        if (sourceEntityId is { IsEmpty: true })
            throw new ArgumentException("A supplied impact source identifier must be valid.", nameof(sourceEntityId));

        VitalityDamage = vitalityDamage;
        PosturePressure = posturePressure;
        SourceEntityId = sourceEntityId;
    }

    public decimal VitalityDamage { get; }
    public decimal PosturePressure { get; }
    /// <summary>Original causal source; preserve it through delayed and environmental damage.</summary>
    public EntityId? SourceEntityId { get; }
}
