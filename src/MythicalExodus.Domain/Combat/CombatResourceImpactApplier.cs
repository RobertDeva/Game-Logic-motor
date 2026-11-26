using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Applies precomputed Vitality damage and Posture pressure as one immutable
/// transition. It owns resource bounds and bar overflow, not combat formulas.
/// </summary>
public sealed class CombatResourceImpactApplier
{
    public Result<CombatImpactResult> Apply(
        CombatantResourceState currentState,
        CombatResourceImpact impact)
    {
        ArgumentNullException.ThrowIfNull(currentState);
        ArgumentNullException.ThrowIfNull(impact);

        var vitality = currentState.Vitality;
        var remainingDamage = impact.VitalityDamage;
        var vitalityDamageApplied = 0m;
        var barsDepleted = 0;

        while (remainingDamage > 0m && !vitality.IsDepleted)
        {
            var activeBar = vitality.CurrentBar!;
            var damageToBar = Math.Min(remainingDamage, activeBar.Current);
            var updatedValue = activeBar.Current - damageToBar;

            var update = vitality.TrySetCurrentBarValue(updatedValue);
            if (update.IsFailure)
                return Result<CombatImpactResult>.Failure(update.Error!);

            vitality = update.Value;
            remainingDamage -= damageToBar;
            vitalityDamageApplied += damageToBar;

            if (vitality.CurrentBar!.Current == 0m)
            {
                var removal = vitality.TryRemoveDepletedCurrentBar();
                if (removal.IsFailure)
                    return Result<CombatImpactResult>.Failure(removal.Error!);

                vitality = removal.Value;
                barsDepleted++;
            }
        }

        var availablePosture = currentState.Posture.Maximum - currentState.Posture.Current;
        var posturePressureApplied = Math.Min(impact.PosturePressure, availablePosture);
        var postureValue = currentState.Posture.Current + posturePressureApplied;
        var postureUpdate = currentState.TrySetPostureValue(postureValue);
        if (postureUpdate.IsFailure)
            return Result<CombatImpactResult>.Failure(postureUpdate.Error!);

        var updatedState = new CombatantResourceState(vitality, postureUpdate.Value.Posture);
        return Result<CombatImpactResult>.Success(new CombatImpactResult(
            updatedState,
            vitalityDamageApplied,
            remainingDamage,
            posturePressureApplied,
            barsDepleted));
    }
}
