namespace MythicalExodus.Domain.Primitives;

/// <summary>An absolute position on the deterministic simulation timeline.</summary>
public readonly record struct SimulationInstant
{
    public SimulationInstant(long tick)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(tick);
        Tick = tick;
    }

    public long Tick { get; }
    public static SimulationInstant Zero => new(0);

    public SimulationInstant Add(SimulationDuration duration) =>
        new(checked(Tick + duration.Ticks));

    public SimulationDuration ElapsedSince(SimulationInstant earlier)
    {
        if (earlier.Tick > Tick)
            throw new ArgumentException("The earlier instant cannot be later than this instant.", nameof(earlier));

        return SimulationDuration.FromTicks(Tick - earlier.Tick);
    }
}
