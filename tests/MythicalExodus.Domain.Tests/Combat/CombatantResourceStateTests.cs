using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class CombatantResourceStateTests
{
    [Fact]
    public void Vitality_and_posture_are_stored_as_separate_resources()
    {
        var vitality = new VitalityTrack([
            new ResourcePool(new ResourceId("vitality-1"), 100m, 100m)
        ]);
        var posture = new ResourcePool(new ResourceId("posture"), 30m, 100m);

        var state = new CombatantResourceState(vitality, posture);

        Assert.Same(vitality, state.Vitality);
        Assert.Same(posture, state.Posture);
    }

    [Fact]
    public void Posture_cannot_share_identifier_with_vitality_bar()
    {
        var vitality = new VitalityTrack([
            new ResourcePool(new ResourceId("shared"), 100m, 100m)
        ]);
        var posture = new ResourcePool(new ResourceId("shared"), 20m, 100m);

        Assert.Throws<ArgumentException>(() => new CombatantResourceState(vitality, posture));
    }

    [Fact]
    public void Posture_requires_a_positive_maximum()
    {
        var vitality = new VitalityTrack([]);
        var posture = new ResourcePool(new ResourceId("posture"), 0m, 0m);

        Assert.Throws<ArgumentException>(() => new CombatantResourceState(vitality, posture));
    }

    [Fact]
    public void Posture_update_is_immutable_and_preserves_vitality()
    {
        var vitality = new VitalityTrack([
            new ResourcePool(new ResourceId("vitality-1"), 70m, 100m)
        ]);
        var posture = new ResourcePool(new ResourceId("posture"), 50m, 100m);
        var original = new CombatantResourceState(vitality, posture);

        var result = original.TrySetPostureValue(25m);

        Assert.True(result.IsSuccess);
        Assert.Equal(25m, result.Value.Posture.Current);
        Assert.Equal(50m, original.Posture.Current);
        Assert.Same(vitality, result.Value.Vitality);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Posture_update_outside_range_is_rejected(decimal value)
    {
        var state = new CombatantResourceState(
            new VitalityTrack([]),
            new ResourcePool(new ResourceId("posture"), 50m, 100m));

        var result = state.TrySetPostureValue(value);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.posture.value_out_of_range", result.Error!.Code);
        Assert.Equal(50m, state.Posture.Current);
    }

    [Fact]
    public void Updating_vitality_preserves_posture_and_original_state()
    {
        var state = new CombatantResourceState(
            new VitalityTrack([
                new ResourcePool(new ResourceId("vitality-1"), 80m, 100m)
            ]),
            new ResourcePool(new ResourceId("posture"), 35m, 100m));

        var result = state.TrySetActiveVitalityValue(45m);

        Assert.True(result.IsSuccess);
        Assert.Equal(45m, result.Value.Vitality.CurrentBar!.Current);
        Assert.Equal(80m, state.Vitality.CurrentBar!.Current);
        Assert.Equal(35m, result.Value.Posture.Current);
    }

    [Fact]
    public void Removing_depleted_vitality_bar_preserves_posture()
    {
        var state = new CombatantResourceState(
            new VitalityTrack([
                new ResourcePool(new ResourceId("vitality-1"), 0m, 100m),
                new ResourcePool(new ResourceId("vitality-2"), 100m, 100m)
            ]),
            new ResourcePool(new ResourceId("posture"), 35m, 100m));

        var result = state.TryRemoveDepletedVitalityBar();

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Vitality.Bars);
        Assert.Equal(100m, result.Value.Vitality.CurrentBar!.Current);
        Assert.Equal(35m, result.Value.Posture.Current);
        Assert.Equal(2, state.Vitality.Bars.Count);
    }
}
