using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class BossPhaseStateTests
{
    private static CombatantResourceState CreateResources(int bars, decimal posture)
    {
        var vitalityBars = Enumerable.Range(1, bars).Select(index =>
            new ResourcePool(new ResourceId($"boss-vitality-{index}"), 100m, 100m));
        return new CombatantResourceState(
            new VitalityTrack(vitalityBars),
            new ResourcePool(new ResourceId("boss-posture"), posture, 100m));
    }

    private static BossPhaseState CreateBoss() =>
        new(new BossPhaseProfile(3, new[] { 2, 3 }), CreateResources(3, 50m));

    [Theory]
    [InlineData(2)]
    [InlineData(6)]
    public void Boss_profile_rejects_phase_counts_outside_three_to_five(int phases)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new BossPhaseProfile(phases));
    }

    [Fact]
    public void Initial_boss_state_starts_in_first_active_phase()
    {
        var boss = CreateBoss();

        Assert.Equal(1, boss.CurrentPhase);
        Assert.Equal(BossPhaseTransitionStatus.Active, boss.Status);
        Assert.False(boss.IsVulnerable);
        Assert.False(boss.IsInvulnerable);
    }

    [Fact]
    public void Losing_one_bar_enters_next_phase_and_retains_forty_percent_posture()
    {
        var boss = CreateBoss();
        var afterBarLoss = boss.Resources.TryConsumeCurrentVitalityBar().Value;

        var result = boss.TryAdvanceAfterBarDepletion(
            afterBarLoss,
            SimulationInstant.Zero,
            SimulationDuration.FromTicks(5),
            SimulationDuration.FromTicks(10));

        Assert.True(result.IsSuccess);
        Assert.Equal(2, result.Value.CurrentPhase);
        Assert.Equal(BossPhaseTransitionStatus.VulnerabilityWindow, result.Value.Status);
        Assert.Equal(20m, result.Value.Resources.Posture.Current);
        Assert.Equal(2, result.Value.Resources.Vitality.Bars.Count);
        Assert.True(result.Value.IsVulnerable);
    }

    [Fact]
    public void Transition_advances_from_vulnerability_to_invulnerability_then_active()
    {
        var boss = CreateBoss();
        var afterBarLoss = boss.Resources.TryConsumeCurrentVitalityBar().Value;
        var phaseTwo = boss.TryAdvanceAfterBarDepletion(
            afterBarLoss,
            SimulationInstant.Zero,
            SimulationDuration.FromTicks(5),
            SimulationDuration.FromTicks(10)).Value;

        var beforeWindowEnd = phaseTwo.AdvanceTo(new SimulationInstant(4));
        var invulnerable = phaseTwo.AdvanceTo(new SimulationInstant(5));
        var active = phaseTwo.AdvanceTo(new SimulationInstant(15));

        Assert.Equal(BossPhaseTransitionStatus.VulnerabilityWindow, beforeWindowEnd.Status);
        Assert.Equal(BossPhaseTransitionStatus.InvulnerableTransition, invulnerable.Status);
        Assert.True(invulnerable.IsInvulnerable);
        Assert.Equal(BossPhaseTransitionStatus.Active, active.Status);
    }

    [Fact]
    public void Special_transition_attack_is_available_only_on_configured_phase_during_invulnerability()
    {
        var boss = CreateBoss();
        var afterBarLoss = boss.Resources.TryConsumeCurrentVitalityBar().Value;
        var phaseTwo = boss.TryAdvanceAfterBarDepletion(
            afterBarLoss,
            SimulationInstant.Zero,
            SimulationDuration.FromTicks(5),
            SimulationDuration.FromTicks(10)).Value;

        Assert.False(phaseTwo.CanUseSpecialTransitionAttack);
        Assert.True(phaseTwo.AdvanceTo(new SimulationInstant(5)).CanUseSpecialTransitionAttack);
        Assert.False(phaseTwo.AdvanceTo(new SimulationInstant(15)).CanUseSpecialTransitionAttack);
    }

    [Fact]
    public void Losing_final_bar_marks_boss_defeated_without_starting_another_transition()
    {
        var boss = CreateBoss();
        var afterFirst = boss.Resources.TryConsumeCurrentVitalityBar().Value;
        var phaseTwo = boss.TryAdvanceAfterBarDepletion(
            afterFirst, SimulationInstant.Zero,
            SimulationDuration.FromTicks(1), SimulationDuration.FromTicks(1)).Value.AdvanceTo(new SimulationInstant(2));
        var afterSecond = phaseTwo.Resources.TryConsumeCurrentVitalityBar().Value;
        var phaseThree = phaseTwo.TryAdvanceAfterBarDepletion(
            afterSecond, new SimulationInstant(2),
            SimulationDuration.FromTicks(1), SimulationDuration.FromTicks(1)).Value.AdvanceTo(new SimulationInstant(4));
        var afterFinal = phaseThree.Resources.TryConsumeCurrentVitalityBar().Value;

        var result = phaseThree.TryAdvanceAfterBarDepletion(
            afterFinal, new SimulationInstant(4),
            SimulationDuration.FromTicks(1), SimulationDuration.FromTicks(1));

        Assert.True(result.IsSuccess);
        Assert.Equal(BossPhaseTransitionStatus.Defeated, result.Value.Status);
        Assert.Equal(3, result.Value.CurrentPhase);
        Assert.Empty(result.Value.Resources.Vitality.Bars);
    }

    [Fact]
    public void Multiple_bars_lost_in_one_impact_are_rejected_instead_of_silently_skipping_phases()
    {
        var boss = CreateBoss();
        var afterTwoBars = boss.Resources.TryConsumeCurrentVitalityBar().Value.TryConsumeCurrentVitalityBar().Value;

        var result = boss.TryAdvanceAfterBarDepletion(
            afterTwoBars, SimulationInstant.Zero,
            SimulationDuration.FromTicks(1), SimulationDuration.FromTicks(1));

        Assert.True(result.IsFailure);
        Assert.Equal("combat.boss.invalid_bar_transition", result.Error!.Code);
    }

    [Fact]
    public void Cannot_advance_phase_while_transition_is_already_active()
    {
        var boss = CreateBoss();
        var afterBarLoss = boss.Resources.TryConsumeCurrentVitalityBar().Value;
        var phaseTwo = boss.TryAdvanceAfterBarDepletion(
            afterBarLoss, SimulationInstant.Zero,
            SimulationDuration.FromTicks(1), SimulationDuration.FromTicks(1)).Value;

        var result = phaseTwo.TryAdvanceAfterBarDepletion(
            phaseTwo.Resources, SimulationInstant.Zero,
            SimulationDuration.FromTicks(1), SimulationDuration.FromTicks(1));

        Assert.True(result.IsFailure);
        Assert.Equal("combat.boss.phase_not_active", result.Error!.Code);
    }
}
