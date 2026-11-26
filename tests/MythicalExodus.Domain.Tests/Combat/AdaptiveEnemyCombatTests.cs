using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class AdaptiveEnemyCombatTests
{
    private static readonly CombatPatternId RepeatedHeavyAttacks = new("player.repeated-heavy-attacks");
    private static readonly CombatPatternId FrequentHealing = new("player.frequent-healing");
    private static readonly CombatPatternId LongRangeKiting = new("player.long-range-kiting");

    private static AdaptiveCombatProfile CreateProfile(AdaptiveCombatLevel level) => new(
        level,
        AdaptiveCombatTendency.Aggressive,
        new[]
        {
            new AdaptiveCombatRule(RepeatedHeavyAttacks, 3, AdaptiveCombatTendency.Defensive, AdaptiveCombatLevel.Basic, 10),
            new AdaptiveCombatRule(FrequentHealing, 2, AdaptiveCombatTendency.Aggressive, AdaptiveCombatLevel.Advanced, 20),
            new AdaptiveCombatRule(LongRangeKiting, 2, AdaptiveCombatTendency.Repositioning, AdaptiveCombatLevel.Expert, 5)
        });

    [Fact]
    public void Level_none_never_adapts_even_when_memory_contains_patterns()
    {
        var memory = new AdaptiveCombatMemory()
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks);

        var decision = memory.Resolve(CreateProfile(AdaptiveCombatLevel.None));

        Assert.True(decision.UsedDefault);
        Assert.Equal(AdaptiveCombatLevel.None, decision.Level);
        Assert.Equal(AdaptiveCombatTendency.Aggressive, decision.Tendency);
    }

    [Fact]
    public void Basic_level_can_use_basic_rules_but_not_advanced_or_expert_rules()
    {
        var memory = new AdaptiveCombatMemory()
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(FrequentHealing)
            .RecordObservation(FrequentHealing)
            .RecordObservation(LongRangeKiting)
            .RecordObservation(LongRangeKiting);

        var decision = memory.Resolve(CreateProfile(AdaptiveCombatLevel.Basic));

        Assert.Equal(AdaptiveCombatTendency.Defensive, decision.Tendency);
        Assert.Equal(RepeatedHeavyAttacks, decision.MatchedPattern);
    }

    [Fact]
    public void Advanced_level_can_use_advanced_rules()
    {
        var memory = new AdaptiveCombatMemory()
            .RecordObservation(FrequentHealing)
            .RecordObservation(FrequentHealing);

        var decision = memory.Resolve(CreateProfile(AdaptiveCombatLevel.Advanced));

        Assert.Equal(AdaptiveCombatTendency.Aggressive, decision.Tendency);
        Assert.Equal(FrequentHealing, decision.MatchedPattern);
    }

    [Fact]
    public void Expert_level_can_use_expert_rules()
    {
        var memory = new AdaptiveCombatMemory()
            .RecordObservation(LongRangeKiting)
            .RecordObservation(LongRangeKiting);

        var decision = memory.Resolve(CreateProfile(AdaptiveCombatLevel.Expert));

        Assert.Equal(AdaptiveCombatTendency.Repositioning, decision.Tendency);
        Assert.Equal(LongRangeKiting, decision.MatchedPattern);
    }

    [Fact]
    public void Highest_priority_eligible_rule_wins_deterministically()
    {
        var memory = new AdaptiveCombatMemory()
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(RepeatedHeavyAttacks)
            .RecordObservation(FrequentHealing)
            .RecordObservation(FrequentHealing);

        var decision = memory.Resolve(CreateProfile(AdaptiveCombatLevel.Advanced));

        Assert.Equal(AdaptiveCombatTendency.Aggressive, decision.Tendency);
        Assert.Equal(FrequentHealing, decision.MatchedPattern);
    }

    [Fact]
    public void Memory_is_immutable_and_survives_phase_changes_when_retained()
    {
        var original = new AdaptiveCombatMemory();
        var learned = original.RecordObservation(LongRangeKiting).RecordObservation(LongRangeKiting);
        var retainedForNextBossPhase = learned;

        Assert.Equal(0, original.GetObservationCount(LongRangeKiting));
        Assert.Equal(2, retainedForNextBossPhase.GetObservationCount(LongRangeKiting));
        Assert.Equal(AdaptiveCombatTendency.Repositioning,
            retainedForNextBossPhase.Resolve(CreateProfile(AdaptiveCombatLevel.Expert)).Tendency);
    }

    [Fact]
    public void Memory_resets_only_when_new_encounter_is_created()
    {
        var memory = new AdaptiveCombatMemory()
            .RecordObservation(FrequentHealing)
            .RecordObservation(FrequentHealing);

        var reset = memory.ResetForNewEncounter();

        Assert.Empty(reset.Observations);
        Assert.Equal(2, memory.GetObservationCount(FrequentHealing));
    }

    [Fact]
    public void Profile_rejects_duplicate_priorities()
    {
        Assert.Throws<ArgumentException>(() => new AdaptiveCombatProfile(
            AdaptiveCombatLevel.Basic,
            AdaptiveCombatTendency.Defensive,
            new[]
            {
                new AdaptiveCombatRule(FrequentHealing, 1, AdaptiveCombatTendency.Aggressive, AdaptiveCombatLevel.Basic, 1),
                new AdaptiveCombatRule(LongRangeKiting, 1, AdaptiveCombatTendency.Repositioning, AdaptiveCombatLevel.Basic, 1)
            }));
    }
}
