namespace Karting.Application.Common;

/// <summary>
/// Represents the Result record.
/// </summary>
public record Result(bool IsSuccess, string? ErrorCode, string? ErrorMessage)
{
    /// <summary>
    /// Executes Success.
    /// </summary>
    /// <returns>The operation result.</returns>
    public static Result Success() => new(true, null, null);

    /// <summary>
    /// Executes Failure.
    /// </summary>
    /// <param name="errorCode">The errorCode value.</param>
    /// <param name="errorMessage">The errorMessage value.</param>
    /// <returns>The operation result.</returns>
    public static Result Failure(string errorCode, string errorMessage) => new(false, errorCode, errorMessage);
}

/// <summary>
/// Represents the Result record.
/// </summary>
public sealed record Result<T>(bool IsSuccess, T? Value, string? ErrorCode, string? ErrorMessage)
{
    /// <summary>
    /// Executes Success.
    /// </summary>
    /// <param name="value">The value value.</param>
    /// <returns>The operation result.</returns>
    public static Result<T> Success(T value) => new(true, value, null, null);

    /// <summary>
    /// Executes Failure.
    /// </summary>
    /// <param name="errorCode">The errorCode value.</param>
    /// <param name="errorMessage">The errorMessage value.</param>
    /// <returns>The operation result.</returns>
    public static Result<T> Failure(string errorCode, string errorMessage) => new(false, default, errorCode, errorMessage);
}
