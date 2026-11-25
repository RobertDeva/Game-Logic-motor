using MythicalExodus.Domain.Primitives;

namespace MythicalExodus.Domain.Tests.Primitives;

public sealed class ResultTests
{
    [Fact]
    public void Success_ExposesValue()
    {
        var result = Result<int>.Success(42);

        Assert.True(result.IsSuccess);
        Assert.Equal(42, result.Value);
        Assert.Null(result.Error);
    }

    [Fact]
    public void Failure_ExposesErrorAndDoesNotExposeValue()
    {
        var result = Result<int>.Failure(new GameError("invalid", "Invalid operation."));

        Assert.True(result.IsFailure);
        Assert.Equal("invalid", result.Error!.Code);
        Assert.Throws<InvalidOperationException>(() => result.Value);
    }
}
