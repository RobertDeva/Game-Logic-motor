using MythicalExodus.Domain.Resources;

namespace MythicalExodus.Domain.Tests.Resources;

public sealed class ResourcePoolTests
{
    [Fact]
    public void ResourcePool_PreservesCurrentAndMaximumValues()
    {
        var pool = new ResourcePool(new ResourceId("mana"), 40m, 100m);

        Assert.Equal("mana", pool.Id.Value);
        Assert.Equal(40m, pool.Current);
        Assert.Equal(100m, pool.Maximum);
    }

    [Fact]
    public void ResourcePool_AllowsAnEmptyPoolAndZeroMaximum()
    {
        var pool = new ResourcePool(new ResourceId("prana"), 0m, 0m);

        Assert.Equal(0m, pool.Current);
        Assert.Equal(0m, pool.Maximum);
    }

    [Theory]
    [InlineData(-1, 100)]
    [InlineData(101, 100)]
    [InlineData(1, -1)]
    public void ResourcePool_RejectsValuesOutsideItsBounds(decimal current, decimal maximum)
    {
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            new ResourcePool(new ResourceId("vitality"), current, maximum));
    }

    [Fact]
    public void ResourcePool_RejectsDefaultIdentifier()
    {
        Assert.Throws<ArgumentException>(() => new ResourcePool(default, 0m, 0m));
    }

    [Fact]
    public void WithCurrent_ReturnsANewValidatedPool()
    {
        var original = new ResourcePool(new ResourceId("aura"), 25m, 100m);

        var updated = original.WithCurrent(10m);

        Assert.Equal(25m, original.Current);
        Assert.Equal(10m, updated.Current);
        Assert.Equal(original.Maximum, updated.Maximum);
    }

    [Fact]
    public void WithMaximum_CannotReduceMaximumBelowCurrent()
    {
        var pool = new ResourcePool(new ResourceId("posture"), 50m, 100m);

        Assert.Throws<ArgumentOutOfRangeException>(() => pool.WithMaximum(40m));
    }
}
