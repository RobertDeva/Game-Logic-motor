using MythicalExodus.Domain.Combat;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Combat;

public sealed class VitalityTrackTests
{
    [Fact]
    public void First_bar_is_active_and_later_bars_remain_full()
    {
        var first = Bar("vitality-1", 40m, 100m);
        var second = Bar("vitality-2", 100m, 100m);
        var track = new VitalityTrack([first, second]);

        Assert.Same(first, track.CurrentBar);
        Assert.Equal(2, track.Bars.Count);
        Assert.False(track.IsDepleted);
    }

    [Fact]
    public void Empty_track_is_depleted()
    {
        var track = new VitalityTrack([]);

        Assert.True(track.IsDepleted);
        Assert.Null(track.CurrentBar);
        Assert.Empty(track.Bars);
    }

    [Fact]
    public void Duplicate_bar_identifiers_are_rejected()
    {
        var first = Bar("same", 100m, 100m);
        var second = Bar("same", 100m, 100m);

        Assert.Throws<ArgumentException>(() => new VitalityTrack([first, second]));
    }

    [Fact]
    public void Zero_maximum_bar_is_rejected()
    {
        var bar = Bar("vitality-1", 0m, 0m);

        Assert.Throws<ArgumentException>(() => new VitalityTrack([bar]));
    }

    [Fact]
    public void Later_bar_cannot_be_partially_depleted_before_becoming_active()
    {
        var first = Bar("vitality-1", 50m, 100m);
        var second = Bar("vitality-2", 90m, 100m);

        Assert.Throws<ArgumentException>(() => new VitalityTrack([first, second]));
    }

    [Fact]
    public void Current_bar_can_only_be_removed_after_it_reaches_zero()
    {
        var track = new VitalityTrack([
            Bar("vitality-1", 1m, 100m),
            Bar("vitality-2", 100m, 100m)
        ]);

        var result = track.TryRemoveDepletedCurrentBar();

        Assert.True(result.IsFailure);
        Assert.Equal("combat.vitality.current_bar_not_depleted", result.Error!.Code);
        Assert.Equal(2, track.Bars.Count);
    }

    [Fact]
    public void Removing_depleted_bar_exposes_next_bar_without_modifying_it()
    {
        var nextBar = Bar("vitality-2", 100m, 100m);
        var track = new VitalityTrack([
            Bar("vitality-1", 0m, 100m),
            nextBar
        ]);

        var result = track.TryRemoveDepletedCurrentBar();

        Assert.True(result.IsSuccess);
        Assert.Single(result.Value.Bars);
        Assert.Same(nextBar, result.Value.CurrentBar);
        Assert.Equal(100m, result.Value.CurrentBar!.Current);
        Assert.Equal(2, track.Bars.Count);
    }

    [Fact]
    public void Removing_final_bar_produces_depleted_track()
    {
        var track = new VitalityTrack([Bar("vitality-1", 0m, 100m)]);

        var result = track.TryRemoveDepletedCurrentBar();

        Assert.True(result.IsSuccess);
        Assert.True(result.Value.IsDepleted);
    }

    [Fact]
    public void Setting_active_bar_value_returns_new_track_without_mutating_original()
    {
        var original = new VitalityTrack([
            Bar("vitality-1", 80m, 100m),
            Bar("vitality-2", 100m, 100m)
        ]);

        var result = original.TrySetCurrentBarValue(35m);

        Assert.True(result.IsSuccess);
        Assert.Equal(35m, result.Value.CurrentBar!.Current);
        Assert.Equal(80m, original.CurrentBar!.Current);
        Assert.Equal(100m, result.Value.Bars[1].Current);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(101)]
    public void Setting_active_bar_outside_range_is_rejected(decimal value)
    {
        var track = new VitalityTrack([Bar("vitality-1", 80m, 100m)]);

        var result = track.TrySetCurrentBarValue(value);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.vitality.value_out_of_range", result.Error!.Code);
        Assert.Equal(80m, track.CurrentBar!.Current);
    }

    [Fact]
    public void Setting_value_on_empty_track_is_rejected()
    {
        var track = new VitalityTrack([]);

        var result = track.TrySetCurrentBarValue(0m);

        Assert.True(result.IsFailure);
        Assert.Equal("combat.vitality.no_remaining_bars", result.Error!.Code);
    }

    private static ResourcePool Bar(string id, decimal current, decimal maximum) =>
        new(new ResourceId(id), current, maximum);
}
