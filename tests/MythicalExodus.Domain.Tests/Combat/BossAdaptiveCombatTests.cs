using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class BossAdaptiveCombatTests
{
    private static readonly CombatPatternId RepeatedHeavyAttacks = new("player.repeated-heavy-attacks");
    private static readonly CombatPatternId FrequentHealing = new("player.frequent-healing");
    private static readonly CombatPatternId LongRangeKiting = new("player.long-range-kiting");

    private static BossAdaptationProfile CreateProfile() => new(
        BossCombatTendency.Aggressive,
        new[]
        {
            new BossAdaptationRule(RepeatedHeavyAttacks, 3, BossCombatTendency.Defensive, priority: 10),
            new BossAdaptationRule(FrequentHealing, 2, BossCombatTendency.Aggressive, priority: 20),
            new BossAdaptationRule(LongRangeKiting, 2, BossCombatTendency.Repositioning, priority: 5)
        });

    [Fact]
    public void Memory_starts_with_no_observations_and_uses_default_tendency()
    {
        var memory = new BossCombatMemory();
        var decision = memory.Resolve(CreateProfile());

        Assert.Empty(memory.Observations);
        Assert.True(decision.UsedDefault);
        Assert.Equal(BossCombatTendency.Aggressive, decision.Tendency);
        Assert.Null(decision.MatchedPattern);
    }

    [Fact]
    public void Repeated_player_pattern_triggers_configured_adaptation()
    {
        var memory = new BossCombatMemory()
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks);

        var decision = memory.Resolve(CreateProfile());

        Assert.False(decision.UsedDefault);
        Assert.Equal(BossCombatTendency.Defensive, decision.Tendency);
        Assert.Equal(RepeatedHeavyAttacks, decision.MatchedPattern);
        Assert.Equal(3, decision.ObservedCount);
    }

    [Fact]
    public void Highest_priority_matching_rule_wins_deterministically()
    {
        var memory = new BossCombatMemory()
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(FrequentHealing)
            .RecordObservation(FrequentHealing);

        var decision = memory.Resolve(CreateProfile());

        Assert.Equal(BossCombatTendency.Aggressive, decision.Tendency);
        Assert.Equal(FrequentHealing, decision.MatchedPattern);
    }

    [Fact]
    public void Memory_is_immutable_when_new_observations_are_recorded()
    {
        var original = new BossCombatMemory();
        var updated = original.RecordObservation(FrequentHealing);

        Assert.Equal(0, original.GetObservationCount(FrequentHealing));
        Assert.Equal(1, updated.GetObservationCount(FrequentHealing));
    }

    [Fact]
    public void Memory_is_independent_of_boss_phase_and_can_be_reused_after_phase_change()
    {
        var memory = new BossCombatMemory()
            .RecordObservation(LongRangeKiting)
            .RecordObservation(LongRangeKiting);

        // The phase state is managed separately; retaining this memory preserves adaptation.
        var nextPhaseMemory = memory;
        var decision = nextPhaseMemory.Resolve(CreateProfile());

        Assert.Equal(2, nextPhaseMemory.GetObservationCount(LongRangeKiting));
        Assert.Equal(BossCombatTendency.Repositioning, decision.Tendency);
    }

    [Fact]
    public void New_encounter_reset_clears_all_learned_patterns()
    {
        var memory = new BossCombatMemory()
            .RecordObservation(FrequentHealing)
            .RecordObservation(FrequentHealing);

        var reset = memory.ResetForNewEncounter();

        Assert.Empty(reset.Observations);
        Assert.Equal(2, memory.GetObservationCount(FrequentHealing));
        Assert.Equal(0, reset.GetObservationCount(FrequentHealing));
    }

    [Fact]
    public void Profile_rejects_duplicate_priorities()
    {
        Assert.Throws<ArgumentException>(() => new BossAdaptationProfile(
            BossCombatTendency.Defensive,
            new[]
            {
                new BossAdaptationRule(FrequentHealing, 1, BossCombatTendency.Aggressive, 1),
                new BossAdaptationRule(LongRangeKiting, 1, BossCombatTendency.Repositioning, 1)
            }));
    }

    [Fact]
    public void Rule_rejects_invalid_threshold()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new BossAdaptationRule(FrequentHealing, 0, BossCombatTendency.Aggressive, 1));
    }
}
