namespace MythicalExodus.Domain.Combat;

/// <summary>Outcome of applying a resolved impact to an immutable combatant state.</summary>
public sealed record CombatImpactResult
{
    public CombatImpactResult(
        CombatantResourceState state,
        decimal vitalityDamageApplied,
        decimal vitalityDamageOverflow,
        decimal posturePressureApplied,
        int vitalityBarsDepleted)
    {
        ArgumentNullException.ThrowIfNull(state);
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityDamageApplied);
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityDamageOverflow);
        ArgumentOutOfRangeException.ThrowIfNegative(posturePressureApplied);
        ArgumentOutOfRangeException.ThrowIfNegative(vitalityBarsDepleted);

        State = state;
        VitalityDamageApplied = vitalityDamageApplied;
        VitalityDamageOverflow = vitalityDamageOverflow;
        PosturePressureApplied = posturePressureApplied;
        VitalityBarsDepleted = vitalityBarsDepleted;
    }

    public CombatantResourceState State { get; }
    public decimal VitalityDamageApplied { get; }
    public decimal VitalityDamageOverflow { get; }
    public decimal PosturePressureApplied { get; }
    public int VitalityBarsDepleted { get; }
    public bool VitalityDepleted => State.Vitality.IsDepleted;
    public bool PostureBroken => State.Posture.Current >= State.Posture.Maximum;
}
