using MythicalExodus.Domain.Conditions;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Conditions;

public sealed class ConditionSetTests
{
    [Fact]
    public void Empty_condition_set_is_satisfied()
    {
        var set = new ConditionSet<int>([]);

        var result = set.Evaluate(42);

        Assert.True(result.IsSatisfied);
        Assert.Empty(result.Failures);
    }

    [Fact]
    public void All_conditions_are_evaluated_in_declaration_order()
    {
        var calls = new List<int>();
        var set = new ConditionSet<int>([
            new RecordingCondition(1, calls, succeeds: false),
            new RecordingCondition(2, calls, succeeds: true),
            new RecordingCondition(3, calls, succeeds: false)
        ]);

        var result = set.Evaluate(0);

        Assert.Equal([1, 2, 3], calls);
        Assert.False(result.IsSatisfied);
        Assert.Equal(["condition.1", "condition.3"], result.Failures.Select(x => x.Error.Code));
    }

    [Fact]
    public void Failure_collection_is_read_only_snapshot()
    {
        var source = new List<ConditionFailure>
        {
            new(new GameError("condition.failed", "Rejected"))
        };

        var evaluation = ConditionEvaluation.FromFailures(source);
        source.Clear();

        Assert.Single(evaluation.Failures);
    }

    [Fact]
    public void Null_condition_is_rejected()
    {
        IGameCondition<int>[] conditions = [null!];

        Assert.Throws<ArgumentException>(() => new ConditionSet<int>(conditions));
    }

    private sealed class RecordingCondition(
        int number,
        List<int> calls,
        bool succeeds) : IGameCondition<int>
    {
        public ConditionEvaluation Evaluate(int context)
        {
            calls.Add(number);
            return succeeds
                ? ConditionEvaluation.Satisfied()
                : ConditionEvaluation.Rejected(new ConditionFailure(
                    new GameError($"condition.{number}", $"Condition {number} failed.")));
        }
    }
}
