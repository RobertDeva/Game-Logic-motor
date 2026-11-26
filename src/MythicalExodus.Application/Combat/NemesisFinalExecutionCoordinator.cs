using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Combat;

/// <summary>
/// Coordinates Nemesis final-execution transitions with persistent progression.
/// The encounter host owns recovery timing and reports when the Nemesis has recovered.
/// </summary>
public sealed class NemesisFinalExecutionCoordinator
{
    private readonly INemesisProgressionRepository _repository;

    public NemesisFinalExecutionCoordinator(INemesisProgressionRepository repository)
    {
        ArgumentNullException.ThrowIfNull(repository);
        _repository = repository;
    }

    /// <summary>
    /// Opens the kneeling opportunity only when content opts this Nemesis in and
    /// the normal Muertebendita execution actually defeated it.
    /// </summary>
    public Result<NemesisFinalExecutionUpdate> OpenOpportunity(
        EntityId nemesisId,
        EntityId encounterId,
        EntityId executionId,
        MuertebenditaExecutionResult normalExecution,
        bool supportsFinalExecution)
    {
        ArgumentNullException.ThrowIfNull(normalExecution);

        if (!supportsFinalExecution)
            return Failure("nemesis.final_execution.not_supported",
                "This Nemesis is not configured to support a final execution.");

        if (!normalExecution.EnemyDefeated)
            return Failure("nemesis.final_execution.target_not_defeated",
                "The final-execution opportunity requires a normal execution that defeated the Nemesis.");

        var current = _repository.Find(nemesisId);
        if (current is null)
            return Failure("nemesis.final_execution.not_found",
                "The persistent Nemesis record could not be found.");

        var transition = current.OpenFinalExecutionOpportunity(encounterId, executionId);
        if (transition.IsFailure)
            return transition;

        return PersistTransition(current, transition.Value);
    }

    /// <summary>Reports recovery after the host determines the Nemesis can return to play.</summary>
    public Result<NemesisFinalExecutionUpdate> Recover(EntityId nemesisId, EntityId executionId)
    {
        var current = _repository.Find(nemesisId);
        if (current is null)
            return Failure("nemesis.final_execution.not_found",
                "The persistent Nemesis record could not be found.");

        var transition = current.RecoverFromFinalExecutionOpportunity(executionId);
        if (transition.IsFailure)
            return transition;

        return PersistTransition(current, transition.Value);
    }

    /// <summary>Persists permanent retirement after the host confirms the contextual final action.</summary>
    public Result<NemesisFinalExecutionUpdate> Complete(EntityId nemesisId, EntityId executionId)
    {
        var current = _repository.Find(nemesisId);
        if (current is null)
            return Failure("nemesis.final_execution.not_found",
                "The persistent Nemesis record could not be found.");

        var transition = current.CompleteFinalExecution(executionId);
        if (transition.IsFailure)
            return transition;

        return PersistTransition(current, transition.Value);
    }

    private Result<NemesisFinalExecutionUpdate> PersistTransition(
        NemesisProgressionState expected,
        NemesisFinalExecutionUpdate transition)
    {
        if (transition.Status == NemesisFinalExecutionUpdateStatus.Duplicate)
            return Result<NemesisFinalExecutionUpdate>.Success(transition);

        if (!_repository.TryUpdate(expected, transition.State))
            return Failure("nemesis.persistence.concurrent_update",
                "The Nemesis record changed during the final-execution transition; retry the same operation.");

        return Result<NemesisFinalExecutionUpdate>.Success(transition);
    }

    private static Result<NemesisFinalExecutionUpdate> Failure(string code, string message) =>
        Result<NemesisFinalExecutionUpdate>.Failure(new GameError(code, message));
}
