namespace Karting.Application.Common;

public record Result(bool IsSuccess, string? ErrorCode, string? ErrorMessage)
{
    public static Result Success() => new(true, null, null);

    public static Result Failure(string errorCode, string errorMessage) => new(false, errorCode, errorMessage);
}

public sealed record Result<T>(bool IsSuccess, T? Value, string? ErrorCode, string? ErrorMessage)
{
    public static Result<T> Success(T value) => new(true, value, null, null);

    public static Result<T> Failure(string errorCode, string errorMessage) => new(false, default, errorCode, errorMessage);
}
