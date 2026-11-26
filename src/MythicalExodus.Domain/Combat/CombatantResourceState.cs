using MythicalExodus.Domain.Primitives;
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

    public Result<CombatantResourceState> TrySetPostureValue(decimal value)
    {
        if (value < 0m || value > Posture.Maximum)
        {
            return Result<CombatantResourceState>.Failure(new GameError(
                "combat.posture.value_out_of_range",
                "The posture value must be between zero and its maximum."));
        }

        return Result<CombatantResourceState>.Success(
            new CombatantResourceState(Vitality, Posture.WithCurrent(value)));
    }

    public Result<CombatantResourceState> TrySetActiveVitalityValue(decimal value)
    {
        var result = Vitality.TrySetCurrentBarValue(value);
        if (result.IsFailure)
            return Result<CombatantResourceState>.Failure(result.Error!);

        return Result<CombatantResourceState>.Success(
            new CombatantResourceState(result.Value, Posture));
    }

    public Result<CombatantResourceState> TryConsumeCurrentVitalityBar()
    {
        var result = Vitality.TryConsumeCurrentBar();
        if (result.IsFailure)
            return Result<CombatantResourceState>.Failure(result.Error!);

        return Result<CombatantResourceState>.Success(
            new CombatantResourceState(result.Value, Posture));
    }

    public Result<CombatantResourceState> TryResetPosture() =>
        TrySetPostureValue(0m);

    public Result<CombatantResourceState> TryRemoveDepletedVitalityBar()
    {
        var result = Vitality.TryRemoveDepletedCurrentBar();
        if (result.IsFailure)
            return Result<CombatantResourceState>.Failure(result.Error!);

        return Result<CombatantResourceState>.Success(
            new CombatantResourceState(result.Value, Posture));
    }
}
