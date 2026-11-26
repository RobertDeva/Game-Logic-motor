using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>Outcome of applying a resolved impact to an immutable combatant state.</summary>
public sealed record CombatImpactResult
{
    public CombatImpactResult(
        CombatantResourceState state,
        decimal vitalityDamageApplied,
        decimal vitalityDamageOverflow,
        decimal posturePressureApplied,
        int vitalityBarsDepleted,
        EntityId? sourceEntityId = null)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityDamageApplied);
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityDamageOverflow);
        ArgumentOutOfRangeException.ThrowIfNegative(posturePressureApplied);
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityBarsDepleted);
        if (sourceEntityId is { IsEmpty: true })
            throw new ArgumentException("A supplied impact source identifier must be valid.", nameof(sourceEntityId));

        State = state;
        VitalityDamageApplied = vitalityDamageApplied;
        VitalityDamageOverflow = vitalityDamageOverflow;
        PosturePressureApplied = posturePressureApplied;
        VitalityBarsDepleted = vitalityBarsDepleted;
        SourceEntityId = sourceEntityId;
    }

    public CombatantResourceState State { get; }
    public decimal VitalityDamageApplied { get; }
    public decimal VitalityDamageOverflow { get; }
    public decimal PosturePressureApplied { get; }
    public int VitalityBarsDepleted { get; }
    public EntityId? SourceEntityId { get; }
    public bool VitalityDepleted => State.Vitality.IsDepleted;
    public bool PostureBroken => State.Posture.Current >= State.Posture.Maximum;
}
