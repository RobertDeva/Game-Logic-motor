namespace MythicalExodus.Domain.Conditions;

/// <summary>
/// A pure predicate over an explicit context. Implementations must not mutate
/// the context or perform hidden game-state changes.
/// </summary>
public interface IGameCondition<in TContext>
{
    ConditionEvaluation Evaluate(TContext context);
}
