using MythicalExodus.Domain.Actions;
using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Conditions;
using MythicalExodus.Domain.Entities;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class CombatActionValidatorTests
{
    [Fact]
    public void Valid_request_is_checked_against_declared_preconditions()
    {
        var actor = CreateCombatant(EntityLifecycleState.Active);
        var target = CreateCombatant(EntityLifecycleState.Active);
        var definition = new ActionDefinition<CombatActionContext>(
            new ActionId("combat.test"),
            new ConditionSet<CombatActionContext>([]));
        var request = new CombatActionRequest(definition.Id, actor.EntityId, target.EntityId);

        var result = new CombatActionValidator().Validate(request, actor, target, definition);

        Assert.True(result.IsSatisfied);
    }

    [Fact]
    public void Actor_identity_mismatch_is_rejected_without_evaluating_action_conditions()
    {
        var actor = CreateCombatant(EntityLifecycleState.Active);
        var otherActor = CreateCombatant(EntityLifecycleState.Active);
        var wasEvaluated = false;
        var definition = new ActionDefinition<CombatActionContext>(
            new ActionId("combat.test"),
            new ConditionSet<CombatActionContext>([new TrackingCondition(() => wasEvaluated = true)]));
        var request = new CombatActionRequest(definition.Id, actor.EntityId);

        var result = new CombatActionValidator().Validate(request, otherActor, null, definition);

        Assert.Contains(result.Failures, x => x.Error.Code == "combat.action.actor_mismatch");
        Assert.False(wasEvaluated);
    }

    [Fact]
    public void Inactive_actor_and_target_are_rejected()
    {
        var actor = CreateCombatant(EntityLifecycleState.Removed);
        var target = CreateCombatant(EntityLifecycleState.Removed);
        var definition = new ActionDefinition<CombatActionContext>(
            new ActionId("combat.test"),
            new ConditionSet<CombatActionContext>([]));
        var request = new CombatActionRequest(definition.Id, actor.EntityId, target.EntityId);

        var result = new CombatActionValidator().Validate(request, actor, target, definition);

        Assert.Contains(result.Failures, x => x.Error.Code == "combat.action.actor_not_active");
        Assert.Contains(result.Failures, x => x.Error.Code == "combat.action.target_not_active");
    }

    [Fact]
    public void Target_presence_must_match_request()
    {
        var actor = CreateCombatant(EntityLifecycleState.Active);
        var target = CreateCombatant(EntityLifecycleState.Active);
        var definition = new ActionDefinition<CombatActionContext>(
            new ActionId("combat.test"),
            new ConditionSet<CombatActionContext>([]));
        var requestWithoutTarget = new CombatActionRequest(definition.Id, actor.EntityId);
        var requestWithTarget = new CombatActionRequest(definition.Id, actor.EntityId, target.EntityId);
        var validator = new CombatActionValidator();

        var unexpected = validator.Validate(requestWithoutTarget, actor, target, definition);
        var missing = validator.Validate(requestWithTarget, actor, null, definition);

        Assert.Equal("combat.action.unexpected_target", unexpected.Failures[0].Error.Code);
        Assert.Equal("combat.action.target_missing", missing.Failures[0].Error.Code);
    }

    [Fact]
    public void Self_target_is_valid_when_identifiers_match()
    {
        var actor = CreateCombatant(EntityLifecycleState.Active);
        var definition = new ActionDefinition<CombatActionContext>(
            new ActionId("combat.test"),
            new ConditionSet<CombatActionContext>([]));
        var request = new CombatActionRequest(definition.Id, actor.EntityId, actor.EntityId);

        var result = new CombatActionValidator().Validate(request, actor, actor, definition);

        Assert.True(result.IsSatisfied);
    }

    private static CombatantSnapshot CreateCombatant(EntityLifecycleState lifecycleState)
    {
        var vitality = new VitalityTrack([
            new ResourcePool(new ResourceId("vitality"), 100m, 100m)
        ]);
        var posture = new ResourcePool(new ResourceId("posture"), 50m, 50m);
        return new CombatantSnapshot(
            EntityId.New(),
            lifecycleState,
            new CombatantResourceState(vitality, posture));
    }

    private sealed class TrackingCondition(Action callback) : IGameCondition<CombatActionContext>
    {
        public ConditionEvaluation Evaluate(CombatActionContext context)
        {
            callback();
            return ConditionEvaluation.Satisfied();
        }
    }
}
