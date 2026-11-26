using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class NemesisProgressionTests
{
    [Theory]
    [InlineData(NemesisEnemyCategory.Normal, 20)]
    [InlineData(NemesisEnemyCategory.Special, 15)]
    [InlineData(NemesisEnemyCategory.Boss, 10)]
    public void Creation_starts_at_rank_one_and_preserves_original_category(
        NemesisEnemyCategory category,
        int expectedMaximumRank)
    {
        var firstEncounter = EntityId.New();
        var state = NemesisProgressionState.Create(EntityId.New(), category, firstEncounter);

        Assert.Equal(1, state.Rank);
        Assert.Equal(category, state.Category);
        Assert.Equal(expectedMaximumRank, state.MaximumRank);
        Assert.Equal(0, state.GrowthMarks);
        Assert.Contains(firstEncounter, state.CreditedEncounterIds);
    }

    [Fact]
    public void Reprocessing_the_same_encounter_does_not_add_growth_or_promote()
    {
        var encounter = EntityId.New();
        var state = NemesisProgressionState.Create(EntityId.New(), NemesisEnemyCategory.Normal, EntityId.New());

        var first = state.RecordQualifyingDefeat(encounter);
        var duplicate = first.State.RecordQualifyingDefeat(encounter);

        Assert.Equal(NemesisProgressionUpdateStatus.RankPromoted, first.Status);
        Assert.Equal(NemesisProgressionUpdateStatus.DuplicateEncounter, duplicate.Status);
        Assert.Equal(2, duplicate.State.Rank);
        Assert.Equal(0, duplicate.State.GrowthMarks);
    }

    [Fact]
    public void Higher_rank_threshold_requires_multiple_distinct_encounters()
    {
        var state = NemesisProgressionState.Create(EntityId.New(), NemesisEnemyCategory.Normal, EntityId.New());

        // Reach rank 6; the threshold from rank 6 to 7 is two marks.
        for (var i = 0; i < 5; i++)
            state = state.RecordQualifyingDefeat(EntityId.New()).State;

        Assert.Equal(6, state.Rank);
        Assert.Equal(2, state.GetRequiredGrowthMarksForNextRank());

        var firstMark = state.RecordQualifyingDefeat(EntityId.New());
        Assert.Equal(NemesisProgressionUpdateStatus.GrowthMarkRecorded, firstMark.Status);
        Assert.Equal(6, firstMark.State.Rank);
        Assert.Equal(1, firstMark.State.GrowthMarks);

        var secondMark = firstMark.State.RecordQualifyingDefeat(EntityId.New());
        Assert.Equal(NemesisProgressionUpdateStatus.RankPromoted, secondMark.Status);
        Assert.Equal(7, secondMark.State.Rank);
        Assert.Equal(0, secondMark.State.GrowthMarks);
    }

    [Fact]
    public void One_encounter_can_never_skip_multiple_ranks()
    {
        var state = NemesisProgressionState.Create(EntityId.New(), NemesisEnemyCategory.Normal, EntityId.New());

        for (var i = 0; i < 5; i++)
            state = state.RecordQualifyingDefeat(EntityId.New()).State;

        Assert.Equal(6, state.Rank);
        Assert.Equal(0, state.GrowthMarks);

        var update = state.RecordQualifyingDefeat(EntityId.New());

        Assert.Equal(6, update.State.Rank);
        Assert.Equal(1, update.State.GrowthMarks);
    }

    [Theory]
    [InlineData(NemesisEnemyCategory.Normal, 20, 46)]
    [InlineData(NemesisEnemyCategory.Special, 15, 27)]
    [InlineData(NemesisEnemyCategory.Boss, 10, 13)]
    public void Escalating_schedule_reaches_each_category_ceiling(
        NemesisEnemyCategory category,
        int expectedRank,
        int expectedLaterDefeats)
    {
        var state = NemesisProgressionState.Create(EntityId.New(), category, EntityId.New());

        for (var i = 0; i < expectedLaterDefeats; i++)
            state = state.RecordQualifyingDefeat(EntityId.New()).State;

        Assert.Equal(expectedRank, state.Rank);
        Assert.True(state.IsAtMaximumRank);
        Assert.Equal(0, state.GrowthMarks);
        Assert.Null(state.GetRequiredGrowthMarksForNextRank());
    }

    [Theory]
    [InlineData(NemesisEnemyCategory.Normal, 46)]
    [InlineData(NemesisEnemyCategory.Special, 27)]
    [InlineData(NemesisEnemyCategory.Boss, 13)]
    public void Defeats_after_rank_cap_do_not_accumulate_growth_marks(
        NemesisEnemyCategory category,
        int defeatsToCap)
    {
        var state = NemesisProgressionState.Create(EntityId.New(), category, EntityId.New());

        for (var i = 0; i < defeatsToCap; i++)
            state = state.RecordQualifyingDefeat(EntityId.New()).State;

        var capped = state.RecordQualifyingDefeat(EntityId.New());

        Assert.Equal(NemesisProgressionUpdateStatus.RankCapReached, capped.Status);
        Assert.Equal(state.Rank, capped.State.Rank);
        Assert.Equal(0, capped.State.GrowthMarks);
    }

    [Fact]
    public void State_is_immutable_when_a_new_defeat_is_recorded()
    {
        var original = NemesisProgressionState.Create(EntityId.New(), NemesisEnemyCategory.Special, EntityId.New());
        var update = original.RecordQualifyingDefeat(EntityId.New());

        Assert.Equal(1, original.Rank);
        Assert.Equal(0, original.GrowthMarks);
        Assert.Equal(2, update.State.Rank);
    }

    [Fact]
    public void Invalid_identifiers_and_categories_are_rejected()
    {
        Assert.Throws<ArgumentException>(() =>
            NemesisProgressionState.Create(default, NemesisEnemyCategory.Normal, EntityId.New()));
        Assert.Throws<ArgumentException>(() =>
            NemesisProgressionState.Create(EntityId.New(), NemesisEnemyCategory.Normal, default));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            NemesisProgressionState.Create(EntityId.New(), (NemesisEnemyCategory)99, EntityId.New()));
    }
}
