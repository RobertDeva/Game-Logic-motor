using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Application.Time;

/// <summary>Provides logical time without exposing wall-clock APIs to domain rules.</summary>
public interface IGameClock
{
    SimulationInstant Now { get; }
}

/// <summary>Advances simulation time explicitly; it never reads the machine clock.</summary>
public sealed class ManualGameClock : IGameClock
{
    public SimulationInstant Now { get; private set; } = SimulationInstant.Zero;

    public void Advance(SimulationDuration duration)
    {
        Now = Now.Add(duration);
    }

    public void Reset() => Now = SimulationInstant.Zero;
}
