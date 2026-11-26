using MythicalExodus.Domain.Combat;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class ResurrectionStateTests
{
    [Fact]
    public void Starts_with_one_available_initial_charge()
    {
        var state = new ResurrectionState(1, 100m);

        Assert.Equal(1, state.MaximumOpportunities);
        Assert.True(state.InitialChargeAvailable);
        Assert.Equal(0, state.AdditionalUsableCharges);
    }

    [Fact]
    public void Missions_unlock_additional_usable_charges_up_to_three()
    {
        var state = new ResurrectionState(1, 100m);
        var firstUnlock = state.TryUnlockAdditionalOpportunity();
        var secondUnlock = firstUnlock.Value.TryUnlockAdditionalOpportunity();

        Assert.True(firstUnlock.IsSuccess);
        Assert.True(secondUnlock.IsSuccess);
        Assert.Equal(3, secondUnlock.Value.MaximumOpportunities);
        Assert.Equal(2, secondUnlock.Value.AdditionalUsableCharges);
        Assert.True(secondUnlock.Value.TryUnlockAdditionalOpportunity().IsFailure);
    }

    [Fact]
    public void Additional_charge_becomes_spent_when_used()
    {
        var state = new ResurrectionState(2, 100m);
        var result = state.TryConsumeAdditionalCharge();

        Assert.True(result.IsSuccess);
        Assert.Equal(0, result.Value.AdditionalUsableCharges);
        Assert.Equal(1, result.Value.AdditionalSpentCharges);
        Assert.Equal(1, state.AdditionalUsableCharges);
    }

    [Fact]
    public void Enemy_defeat_progress_restores_spent_charge_but_keeps_it_locked()
    {
        var state = new ResurrectionState(2, 100m).TryConsumeAdditionalCharge().Value;
        var result = state.TryRecordEnemyDefeat(100m);

        Assert.True(result.IsSuccess);
        Assert.Equal(1, result.Value.AdditionalLockedCharges);
        Assert.Equal(0, result.Value.AdditionalUsableCharges);
        Assert.Equal(0, result.Value.AdditionalSpentCharges);
    }

    [Fact]
    public void Progress_can_accumulate_while_additional_charges_are_unusable()
    {
        var state = new ResurrectionState(2, 100m).TryConsumeAdditionalCharge().Value;
        var result = state.TryRecordEnemyDefeat(60m);

        Assert.True(result.IsSuccess);
        Assert.Equal(60m, result.Value.DefeatProgress);
        Assert.Equal(1, result.Value.AdditionalSpentCharges);
    }

    [Fact]
    public void Partial_progress_is_carried_between_enemy_defeats()
    {
        var state = new ResurrectionState(2, 100m).TryConsumeAdditionalCharge().Value;
        state = state.TryRecordEnemyDefeat(60m).Value;
        state = state.TryRecordEnemyDefeat(50m).Value;

        Assert.Equal(1, state.AdditionalLockedCharges);
        Assert.Equal(0, state.AdditionalSpentCharges);
        Assert.Equal(0m, state.DefeatProgress);
    }

    [Fact]
    public void Excess_progress_can_restore_multiple_spent_charges()
    {
        var state = new ResurrectionState(3, 100m)
            .TryConsumeAdditionalCharge().Value
            .TryConsumeAdditionalCharge().Value;
        var result = state.TryRecordEnemyDefeat(250m);

        Assert.Equal(2, result.Value.AdditionalLockedCharges);
        Assert.Equal(0, result.Value.AdditionalSpentCharges);
        Assert.Equal(0m, result.Value.DefeatProgress);
    }

    [Fact]
    public void Restores_initial_charge_and_unlocks_recovered_additional_charges()
    {
        var state = new ResurrectionState(3, 100m)
            .TryConsumeInitialCharge().Value
            .TryConsumeAdditionalCharge().Value
            .TryRecordEnemyDefeat(100m).Value;
        var rested = state.ReturnToRestPoint();

        Assert.True(rested.InitialChargeAvailable);
        Assert.Equal(2, rested.AdditionalUsableCharges);
        Assert.Equal(0, rested.AdditionalLockedCharges);
        Assert.Equal(0, rested.AdditionalSpentCharges);
    }

    [Fact]
    public void Rest_does_not_grant_unrecovered_additional_charges()
    {
        var state = new ResurrectionState(2, 100m).TryConsumeAdditionalCharge().Value;
        var rested = state.ReturnToRestPoint();

        Assert.Equal(0, rested.AdditionalUsableCharges);
        Assert.Equal(1, rested.AdditionalSpentCharges);
    }

    [Fact]
    public void Does_not_accumulate_progress_when_no_additional_charge_is_spent()
    {
        var state = new ResurrectionState(2, 100m);
        var result = state.TryRecordEnemyDefeat(80m);

        Assert.True(result.IsSuccess);
        Assert.Equal(0m, result.Value.DefeatProgress);
    }

    [Fact]
    public void Rejects_non_positive_enemy_progress()
    {
        var state = new ResurrectionState(2, 100m);

        Assert.Equal(
            "resurrection.progress.invalid_contribution",
            state.TryRecordEnemyDefeat(0m).Error!.Code);
    }

    [Fact]
    public void Initial_charge_is_not_automatically_recharged_by_enemy_defeats()
    {
        var state = new ResurrectionState(1, 100m).TryConsumeInitialCharge().Value;
        var afterDefeat = state.TryRecordEnemyDefeat(100m).Value;

        Assert.False(afterDefeat.InitialChargeAvailable);
        Assert.True(afterDefeat.ReturnToRestPoint().InitialChargeAvailable);
    }
}
