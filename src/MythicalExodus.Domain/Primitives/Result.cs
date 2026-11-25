namespace MythicalExodus.Domain.Primitives;

/// <summary>Represents an expected success or failure without using exceptions for control flow.</summary>
public sealed class Result<T>
{
    private readonly T? _value;

    private Result(bool isSuccess, T? value, GameError? error)
    {
        IsSuccess = isSuccess;
        _value = value;
        Error = error;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public GameError? Error { get; }

    public T Value => IsSuccess
        ? _value!
        : throw new InvalidOperationException("A failed result has no value.");

    public static Result<T> Success(T value) => new(true, value, null);

    public static Result<T> Failure(GameError error)
    {
        ArgumentNullException.ThrowIfNull(error);
        return new(false, default, error);
    }
}

/// <summary>A stable, machine-readable error returned by a game operation.</summary>
public sealed record GameError
{
    public GameError(string code, string message)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);
        Code = code;
        Message = message;
    }

    public string Code { get; }
    public string Message { get; }
}
