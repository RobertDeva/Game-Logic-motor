using MythicalExodus.Domain.Actions;
using MythicalExodus.Domain.Conditions;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Actions;

public sealed class ActionDefinitionTests
{
    [Fact]
    public void Action_definition_exposes_its_identifier_and_validates_preconditions()
    {
        var conditions = new ConditionSet<int>([
            new MinimumValueCondition(10)
        ]);
        var action = new ActionDefinition<int>(new ActionId("combat.heavy_attack"), conditions);

        var accepted = action.Validate(12);
        var rejected = action.Validate(4);

        Assert.Equal("combat.heavy_attack", action.Id.Value);
        Assert.True(accepted.IsSatisfied);
        Assert.False(rejected.IsSatisfied);
        Assert.Equal("condition.minimum_not_met", rejected.Failures[0].Error.Code);
    }

    [Fact]
    public void Invalid_action_identifier_is_rejected()
    {
        Assert.Throws<ArgumentException>(() =>
            new ActionDefinition<int>(default, new ConditionSet<int>([])));
    }

    private sealed class MinimumValueCondition(int minimum) : IGameCondition<int>
    {
        public ConditionEvaluation Evaluate(int context) =>
            context >= minimum
                ? ConditionEvaluation.Satisfied()
                : ConditionEvaluation.Rejected(new ConditionFailure(
                    new GameError("condition.minimum_not_met", "The minimum value was not met.")));
    }
}
