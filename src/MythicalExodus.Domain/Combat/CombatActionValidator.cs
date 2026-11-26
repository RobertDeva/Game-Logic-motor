using MythicalExodus.Domain.Actions;
using MythicalExodus.Domain.Conditions;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Validates request identity, simulation participation and declared action
/// preconditions. It never applies costs, effects or resource transitions.
/// </summary>
public sealed class CombatActionValidator
{
    public ConditionEvaluation Validate(
        CombatActionRequest request,
        CombatantSnapshot actor,
        CombatantSnapshot? target,
        ActionDefinition<CombatActionContext> definition)
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(actor);
        ArgumentNullException.ThrowIfNull(definition);

        var failures = new List<ConditionFailure>();

        if (request.ActionId != definition.Id)
        {
            failures.Add(Failure(
                "combat.action.definition_mismatch",
                "The requested action does not match the supplied action definition."));
        }

        if (request.ActorId != actor.EntityId)
        {
            failures.Add(Failure(
                "combat.action.actor_mismatch",
                "The supplied actor does not match the request actor identifier."));
        }

        if (!actor.IsActive)
        {
            failures.Add(Failure(
                "combat.action.actor_not_active",
                "The actor is not participating in the current simulation."));
        }

        if (request.TargetId is null && target is not null)
        {
            failures.Add(Failure(
                "combat.action.unexpected_target",
                "A target was supplied for an action request without a target identifier."));
        }
        else if (request.TargetId is not null && target is null)
        {
            failures.Add(Failure(
                "combat.action.target_missing",
                "The requested target snapshot was not supplied."));
        }
        else if (request.TargetId is not null && target is not null)
        {
            if (request.TargetId.Value != target.EntityId)
            {
                failures.Add(Failure(
                    "combat.action.target_mismatch",
                    "The supplied target does not match the request target identifier."));
            }

            if (!target.IsActive)
            {
                failures.Add(Failure(
                    "combat.action.target_not_active",
                    "The target is not participating in the current simulation."));
            }
        }

        if (failures.Count > 0)
            return ConditionEvaluation.FromFailures(failures);

        return definition.Validate(new CombatActionContext(actor, target));
    }

    private static ConditionFailure Failure(string code, string message) =>
        new(new GameError(code, message));
}
