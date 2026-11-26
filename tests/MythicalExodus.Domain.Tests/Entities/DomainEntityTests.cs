using MythicalExodus.Domain.Entities;
using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Entities;

public sealed class DomainEntityTests
{
    [Fact]
    public void NewEntity_StartsActive()
    {
        var entity = new TestEntity(EntityId.New());

        Assert.True(entity.IsActive);
        Assert.Equal(EntityLifecycleState.Active, entity.LifecycleState);
    }

    [Fact]
    public void Remove_TransitionsEntityOnlyOnce()
    {
        var entity = new TestEntity(EntityId.New());

        Assert.True(entity.Remove());
        Assert.False(entity.Remove());
        Assert.False(entity.IsActive);
        Assert.Equal(EntityLifecycleState.Removed, entity.LifecycleState);
    }

    [Fact]
    public void EntitiesWithSameId_AreEqual()
    {
        var id = EntityId.New();

        Assert.Equal(new TestEntity(id), new TestEntity(id));
    }

    [Fact]
    public void Constructor_RejectsEmptyId()
    {
        Assert.Throws<ArgumentException>(() => new TestEntity(default));
    }

    private sealed class TestEntity : DomainEntity
    {
        public TestEntity(EntityId id) : base(id) { }
    }
}
