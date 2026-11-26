using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Resource state shared by combatants: segmented Vitality and a separate Posture pool.
/// Resource operation policies remain in dedicated combat rules.
/// </summary>
public sealed record CombatantResourceState
{
    public CombatantResourceState(VitalityTrack vitality, ResourcePool posture)
    {
        ArgumentNullException.ThrowIfNull(vitality);
        ArgumentNullException.ThrowIfNull(posture);

        if (posture.Maximum <= 0m)
            throw new ArgumentException("A posture pool must have a positive maximum.", nameof(posture));
        if (vitality.Bars.Any(bar => bar.Id == posture.Id))
            throw new ArgumentException(
                "Posture must use a different resource identifier from every Vitality bar.",
                nameof(posture));

        Vitality = vitality;
        Posture = posture;
    }

    public VitalityTrack Vitality { get; }
    public ResourcePool Posture { get; }
}
