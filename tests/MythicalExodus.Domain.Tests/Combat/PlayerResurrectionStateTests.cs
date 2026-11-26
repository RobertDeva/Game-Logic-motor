using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Statistics;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class PlayerResurrectionStateTests
{
    private static readonly EntityId Checkpoint = new(Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"));
    private static ResurrectionDebuffDefinition CreateDebuff(long duration = 50) =>
        new(SimulationDuration.FromTicks(duration), new[]
        {
            new StatModifier(
                new EntityId(Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")),
                new EntityId(Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc")),
                new StatId("strength"),
                StatModifierOperation.Multiplier,
                0.9m)
        });

    [Fact]
    public void Lethal_event_opens_a_voluntary_decision_when_a_charge_is_available()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint);
        var result = player.TryBeginLethalEvent();
        Assert.True(result.IsSuccess);
        Assert.Equal(PlayerResurrectionPhase.AwaitingDecision, result.Value.Phase);
        Assert.True(result.Value.CanChooseResurrection);
        Assert.True(result.Value.Charges.InitialChargeAvailable);
    }

    [Fact]
    public void Lethal_event_enters_game_over_when_no_charge_is_usable()
    {
        var charges = new ResurrectionState(1, 100m).TryConsumeInitialCharge().Value;
        var result = new PlayerResurrectionState(charges, Checkpoint).TryBeginLethalEvent();
        Assert.True(result.IsSuccess);
        Assert.Equal(PlayerResurrectionPhase.GameOver, result.Value.Phase);
        Assert.Equal(Checkpoint, result.Value.LastRestPointId);
    }

    [Fact]
    public void Player_can_decline_resurrection_and_enter_game_over()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint).TryBeginLethalEvent().Value;
        var result = player.TryAcceptDeath();
        Assert.True(result.IsSuccess);
        Assert.Equal(PlayerResurrectionPhase.GameOver, result.Value.Phase);
    }

    [Fact]
    public void Voluntary_resurrection_consumes_selected_initial_charge_and_applies_timed_debuff()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint).TryBeginLethalEvent().Value;
        var result = player.TryResurrect(ResurrectionChargeKind.Initial, CreateDebuff(), new SimulationInstant(10));
        Assert.True(result.IsSuccess);
        Assert.Equal(PlayerResurrectionPhase.Alive, result.Value.Phase);
        Assert.False(result.Value.Charges.InitialChargeAvailable);
        Assert.NotNull(result.Value.ActiveDebuff);
        Assert.Equal(10, result.Value.ActiveDebuff!.StartedAt.Tick);
        Assert.Equal(60, result.Value.ActiveDebuff.ExpiresAt.Tick);
        Assert.Single(result.Value.ActiveDebuff.ModifiersAt(new SimulationInstant(59)));
        Assert.Empty(result.Value.ActiveDebuff.ModifiersAt(new SimulationInstant(60)));
    }

    [Fact]
    public void Player_can_choose_additional_charge_without_consuming_initial_charge()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(2, 100m), Checkpoint).TryBeginLethalEvent().Value;
        var result = player.TryResurrect(ResurrectionChargeKind.Additional, CreateDebuff(), SimulationInstant.Zero);
        Assert.True(result.IsSuccess);
        Assert.True(result.Value.Charges.InitialChargeAvailable);
        Assert.Equal(0, result.Value.Charges.AdditionalUsableCharges);
        Assert.Equal(1, result.Value.Charges.AdditionalSpentCharges);
    }

    [Fact]
    public void Cannot_resurrect_without_pending_decision()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint);
        var result = player.TryResurrect(ResurrectionChargeKind.Initial, CreateDebuff(), SimulationInstant.Zero);
        Assert.True(result.IsFailure);
        Assert.Equal("resurrection.decision_not_pending", result.Error!.Code);
    }

    [Fact]
    public void Cannot_select_a_charge_that_is_not_usable()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint).TryBeginLethalEvent().Value;
        var result = player.TryResurrect(ResurrectionChargeKind.Additional, CreateDebuff(), SimulationInstant.Zero);
        Assert.True(result.IsFailure);
        Assert.Equal("resurrection.additional_charge_unavailable", result.Error!.Code);
        Assert.Equal(PlayerResurrectionPhase.AwaitingDecision, player.Phase);
    }

    [Fact]
    public void Returning_after_game_over_restores_checkpoint_and_only_recharges_according_to_rest_rules()
    {
        var charges = new ResurrectionState(2, 100m).TryConsumeInitialCharge().Value.TryConsumeAdditionalCharge().Value;
        var player = new PlayerResurrectionState(charges, Checkpoint).TryBeginLethalEvent().Value;
        var result = player.TryReturnAfterGameOver();
        Assert.True(result.IsSuccess);
        Assert.Equal(PlayerResurrectionPhase.Alive, result.Value.Phase);
        Assert.Equal(Checkpoint, result.Value.LastRestPointId);
        Assert.True(result.Value.Charges.InitialChargeAvailable);
        Assert.Equal(0, result.Value.Charges.AdditionalUsableCharges);
        Assert.Equal(1, result.Value.Charges.AdditionalSpentCharges);
    }

    [Fact]
    public void Rest_updates_checkpoint_and_unlocks_recovered_additional_charges()
    {
        var nextCheckpoint = new EntityId(Guid.Parse("dddddddd-dddd-dddd-dddd-dddddddddddd"));
        var charges = new ResurrectionState(2, 100m).TryConsumeAdditionalCharge().Value.TryRecordEnemyDefeat(100m).Value;
        var result = new PlayerResurrectionState(charges, Checkpoint).TryRestAt(nextCheckpoint);
        Assert.True(result.IsSuccess);
        Assert.Equal(nextCheckpoint, result.Value.LastRestPointId);
        Assert.Equal(1, result.Value.Charges.AdditionalUsableCharges);
        Assert.Equal(0, result.Value.Charges.AdditionalLockedCharges);
    }

    [Fact]
    public void Player_state_forwards_enemy_progress_and_mission_unlocks()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint);
        var unlocked = player.TryUnlockAdditionalOpportunity();
        var spent = unlocked.Value.Charges.TryConsumeAdditionalCharge().Value;
        var spentPlayer = new PlayerResurrectionState(spent, Checkpoint);
        var progress = spentPlayer.TryRecordEnemyDefeat(40m);
        Assert.True(unlocked.IsSuccess);
        Assert.True(progress.IsSuccess);
        Assert.Equal(40m, progress.Value.Charges.DefeatProgress);
    }

    [Fact]
    public void Debuff_expires_using_logical_simulation_time()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint)
            .TryBeginLethalEvent().Value
            .TryResurrect(ResurrectionChargeKind.Initial, CreateDebuff(5), SimulationInstant.Zero).Value;
        var beforeExpiry = player.AdvanceTo(new SimulationInstant(4));
        var atExpiry = player.AdvanceTo(new SimulationInstant(5));
        Assert.NotNull(beforeExpiry.ActiveDebuff);
        Assert.Null(atExpiry.ActiveDebuff);
    }

    [Fact]
    public void Rest_is_rejected_while_player_is_waiting_for_resurrection_decision()
    {
        var player = new PlayerResurrectionState(new ResurrectionState(1, 100m), Checkpoint).TryBeginLethalEvent().Value;
        var result = player.TryRestAt(Checkpoint);
        Assert.True(result.IsFailure);
        Assert.Equal("resurrection.rest_requires_alive_player", result.Error!.Code);
    }
}