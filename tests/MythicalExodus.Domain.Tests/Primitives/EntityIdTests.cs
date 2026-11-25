using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Primitives;

public sealed class EntityIdTests
{
    [Fact]
    public void New_CreatesNonEmptyDistinctIdentities()
    {
        var first = EntityId.New();
        var second = EntityId.New();

        Assert.False(first.IsEmpty);
        Assert.NotEqual(first, second);
    }
}
