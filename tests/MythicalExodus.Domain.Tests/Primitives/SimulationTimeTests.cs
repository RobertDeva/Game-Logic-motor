using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Primitives;

public sealed class SimulationTimeTests
{
    [Fact]
    public void Instant_AddsDurationAndCalculatesElapsedTime()
    {
        var start = new SimulationInstant(10);
        var end = start.Add(SimulationDuration.FromTicks(25));

        Assert.Equal(35, end.Tick);
        Assert.Equal(25, end.ElapsedSince(start).Ticks);
    }

    [Fact]
    public void Duration_RejectsNegativeTicks()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => SimulationDuration.FromTicks(-1));
    }

    [Fact]
    public void Instant_RejectsNegativeTick()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new SimulationInstant(-1));
    }
}
