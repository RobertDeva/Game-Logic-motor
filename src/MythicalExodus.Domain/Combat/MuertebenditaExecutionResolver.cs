namespace MythicalExodus.Domain.Combat;

using MythicalExodus.Domain.Primitives;

public enum MuertebenditaResistance
{
    Normal = 0,
    Resistant = 1
}

public sealed record MuertebenditaExecutionResult(
    CombatantResourceState State,
    bool EnemyDefeated,
    int VitalityBarsConsumed,
    bool PostureReset);

/// <summary>
/// Resolves an explicitly requested Muertebendita execution. Eligibility is
/// enabled by broken Posture or fully depleted Vitality; it never executes automatically.
/// </summary>
public sealed class MuertebenditaExecutionResolver
{
    public Result<MuertebenditaExecutionResult> Execute(
        CombatantResourceState targetState,
        MuertebenditaResistance resistance)
    {
        ArgumentNullException.ThrowIfNull(targetState);
        if (!Enum.IsDefined(resistance))
        {
            return Failure(
                "combat.execution.invalid_resistance",
                "The target execution resistance is not defined.");
        }

        var isPostureBroken = targetState.Posture.Current >= targetState.Posture.Maximum;
        var isVitalityDepleted = targetState.Vitality.IsDepleted;
        if (!isPostureBroken && !isVitalityDepleted)
        {
            return Failure(
                "combat.execution.target_not_vulnerable",
                "The target is not vulnerable to a Muertebendita execution.");
        }

        if (resistance == MuertebenditaResistance.Normal)
        {
            return Result<MuertebenditaExecutionResult>.Success(new MuertebenditaExecutionResult(
                targetState,
                EnemyDefeated: true,
                VitalityBarsConsumed: 0,
                PostureReset: false));
        }

        if (isVitalityDepleted)
        {
            var resetDepletedPosture = targetState.TryResetPosture();
            if (resetDepletedPosture.IsFailure)
                return Result<MuertebenditaExecutionResult>.Failure(resetDepletedPosture.Error!);

            return Result<MuertebenditaExecutionResult>.Success(new MuertebenditaExecutionResult(
                resetDepletedPosture.Value,
                EnemyDefeated: true,
                VitalityBarsConsumed: 0,
                PostureReset: true));
        }

        var consumeBar = targetState.TryConsumeCurrentVitalityBar();
        if (consumeBar.IsFailure)
            return Result<MuertebenditaExecutionResult>.Failure(consumeBar.Error!);

        var resetPosture = consumeBar.Value.TryResetPosture();
        if (resetPosture.IsFailure)
            return Result<MuertebenditaExecutionResult>.Failure(resetPosture.Error!);

        return Result<MuertebenditaExecutionResult>.Success(new MuertebenditaExecutionResult(
            resetPosture.Value,
            EnemyDefeated: resetPosture.Value.Vitality.IsDepleted,
            VitalityBarsConsumed: 1,
            PostureReset: true));
    }

    private static Result<MuertebenditaExecutionResult> Failure(string code, string message) =>
        Result<MuertebenditaExecutionResult>.Failure(new GameError(code, message));
}
