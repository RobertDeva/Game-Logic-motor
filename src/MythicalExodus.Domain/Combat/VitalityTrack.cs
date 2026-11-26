using MythicalExodus.Domain.Primitives;
using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Combat;

/// <summary>
/// Ordered vitality bars for a combatant. Only the first bar may be partially
/// depleted; later bars remain untouched until they become active.
/// </summary>
public sealed class VitalityTrack
{
    private readonly ResourcePool[] _bars;
    private readonly IReadOnlyList<ResourcePool> _readOnlyBars;

    public VitalityTrack(IEnumerable<ResourcePool> bars)
    {
        ArgumentNullException.ThrowIfNull(bars);
        _bars = bars.ToArray();
        var identifiers = new HashSet<ResourceId>();

        for (var index = 0; index < _bars.Length; index++)
        {
            var bar = _bars[index] ??
                throw new ArgumentException("A vitality track cannot contain a null bar.", nameof(bars));

            if (bar.Maximum <= 0m)
                throw new ArgumentException("A vitality bar must have a positive maximum.", nameof(bars));
            if (!identifiers.Add(bar.Id))
                throw new ArgumentException("Vitality bar identifiers must be unique.", nameof(bars));
            if (index > 0 && bar.Current != bar.Maximum)
                throw new ArgumentException("Only the active vitality bar may be partially depleted.", nameof(bars));
        }

        _readOnlyBars = Array.AsReadOnly(_bars);
    }

    public IReadOnlyList<ResourcePool> Bars => _readOnlyBars;
    public bool IsDepleted => _bars.Length == 0;
    public ResourcePool? CurrentBar => IsDepleted ? null : _bars[0];

    /// <summary>Returns a new track with the active bar set to a validated value.</summary>
    public Result<VitalityTrack> TrySetCurrentBarValue(decimal value)
    {
        if (IsDepleted)
        {
            return Result<VitalityTrack>.Failure(new GameError(
                "combat.vitality.no_remaining_bars",
                "The vitality track has no remaining bars."));
        }

        if (value < 0m || value > _bars[0].Maximum)
        {
            return Result<VitalityTrack>.Failure(new GameError(
                "combat.vitality.value_out_of_range",
                "The active vitality value must be between zero and its maximum."));
        }

        var updatedBars = (ResourcePool[])_bars.Clone();
        updatedBars[0] = _bars[0].WithCurrent(value);
        return Result<VitalityTrack>.Success(new VitalityTrack(updatedBars));
    }

    /// <summary>Consumes the active bar in full, independently of its remaining value.</summary>
    public Result<VitalityTrack> TryConsumeCurrentBar()
    {
        if (IsDepleted)
        {
            return Result<VitalityTrack>.Failure(new GameError(
                "combat.vitality.no_remaining_bars",
                "The vitality track has no remaining bars."));
        }

        return Result<VitalityTrack>.Success(new VitalityTrack(_bars.Skip(1)));
    }

    /// <summary>Removes the active bar after it reaches zero.</summary>
    public Result<VitalityTrack> TryRemoveDepletedCurrentBar()
    {
        if (IsDepleted)
        {
            return Result<VitalityTrack>.Failure(new GameError(
                "combat.vitality.no_remaining_bars",
                "The vitality track has no remaining bars."));
        }

        if (_bars[0].Current != 0m)
        {
            return Result<VitalityTrack>.Failure(new GameError(
                "combat.vitality.current_bar_not_depleted",
                "The active vitality bar must reach zero before it can be removed."));
        }

        return Result<VitalityTrack>.Success(new VitalityTrack(_bars.Skip(1)));
    }
}
