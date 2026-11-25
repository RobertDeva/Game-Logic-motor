namespace MythicalExodus.Domain.Primitives;

/// <summary>A non-negative duration measured in simulation ticks.</summary>
public readonly record struct SimulationDuration
{
    private SimulationDuration(long ticks) => Ticks = ticks;

    public long Ticks { get; }
    public static SimulationDuration Zero => new(0);

    public static SimulationDuration FromTicks(long ticks)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(ticks);
        return new SimulationDuration(ticks);
    }

    public static SimulationDuration operator +(SimulationDuration left, SimulationDuration right) =>
        FromTicks(checked(left.Ticks + right.Ticks));
}
