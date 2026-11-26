using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class MuertebenditaExecutionResolverTests
{
    private readonly MuertebenditaExecutionResolver _resolver = new();

    [Fact]
    public void Target_is_not_vulnerable_until_posture_break_or_vitality_depletion()
    {
        var state = CreateState([100m], 50m, posture: 49m);

        var result = _resolver.Execute(state, MuertebenditaResistance.Normal);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.execution.target_not_vulnerable", result.Error!.Code);
    }

    [Fact]
    public void Normal_enemy_is_defeated_by_manual_execution_after_posture_break()
    {
        var state = CreateState([100m], 50m, posture: 50m);

        var result = _resolver.Execute(state, MuertebenditaResistance.Normal);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.EnemyDefeated);
        Assert.Equal(0, result.Value.VitalityBarsConsumed);
        Assert.Same(state, result.Value.State);
    }

    [Fact]
    public void Fully_depleted_vitality_also_enables_execution()
    {
        var state = CreateState(Array.Empty<decimal>(), 0m, posture: 0m);

        var result = _resolver.Execute(state, MuertebenditaResistance.Normal);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.EnemyDefeated);
    }

    [Fact]
    public void Resistant_enemy_execution_consumes_one_entire_active_vitality_bar_and_resets_posture()
    {
        var state = CreateState([100m, 80m, 60m], 35m, posture: 50m);

        var result = _resolver.Execute(state, MuertebenditaResistance.Resistant);

        Assert.True(result.IsSuccess);
        Assert.False(result.Value.EnemyDefeated);
        Assert.Equal(1, result.Value.VitalityBarsConsumed);
        Assert.True(result.Value.PostureReset);
        Assert.Equal(2, result.Value.State.Vitality.Bars.Count);
        Assert.Equal(80m, result.Value.State.Vitality.CurrentBar!.Current);
        Assert.Equal(0m, result.Value.State.Posture.Current);
        Assert.Equal(35m, state.Vitality.CurrentBar!.Current);
        Assert.Equal(50m, state.Posture.Current);
    }

    [Fact]
    public void Resistant_enemy_requires_one_manual_execution_per_remaining_bar()
    {
        var state = CreateState([100m, 80m, 60m], 100m, posture: 50m);

        var first = _resolver.Execute(state, MuertebenditaResistance.Resistant);
        var second = _resolver.Execute(
            CreateState(first.Value.State.Vitality.Bars.Select(bar => bar.Maximum).ToArray(), 80m, posture: 50m),
            MuertebenditaResistance.Resistant);
        var third = _resolver.Execute(
            CreateState(second.Value.State.Vitality.Bars.Select(bar => bar.Maximum).ToArray(), 60m, posture: 50m),
            MuertebenditaResistance.Resistant);

        Assert.False(first.Value.EnemyDefeated);
        Assert.False(second.Value.EnemyDefeated);
        Assert.True(third.Value.EnemyDefeated);
        Assert.Empty(third.Value.State.Vitality.Bars);
        Assert.Equal(0m, third.Value.State.Posture.Current);
    }

    [Fact]
    public void Resistant_enemy_with_already_depleted_vitality_is_defeated_by_execution()
    {
        var state = CreateState(Array.Empty<decimal>(), 0m, posture: 0m);

        var result = _resolver.Execute(state, MuertebenditaResistance.Resistant);

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.EnemyDefeated);
        Assert.Equal(0, result.Value.VitalityBarsConsumed);
        Assert.True(result.Value.PostureReset);
        Assert.Equal(0m, result.Value.State.Posture.Current);
    }

    [Fact]
    public void Unknown_resistance_profile_is_rejected()
    {
        var state = CreateState([100m], 100m, posture: 50m);

        var result = _resolver.Execute(state, (MuertebenditaResistance)999);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.execution.invalid_resistance", result.Error!.Code);
    }

    private static CombatantResourceState CreateState(decimal[] maxima, decimal current, decimal posture)
    {
        var bars = maxima.Select((maximum, index) =>
            new ResourcePool(new ResourceId($"vitality-{index}"), index == 0 ? current : maximum, maximum));
        return new CombatantResourceState(
            new VitalityTrack(bars),
            new ResourcePool(new ResourceId("posture"), posture, 50m));
    }
}
