using System.Net;

namespace Tajan.Standard.Domain.Wrappers;


//Imutable record to prevent changing value after i set it!
public record Error(string Code, string Message)
{
    //Best practise: make it readonly
    public static readonly Error None = new(string.Empty, string.Empty);
    public static readonly Error NullValue = new("Error.NullValue", "A null value was encountered.");
    public static readonly Error NotFoundItem = new("Error.NotFound", "Nothing Found!");
    public static readonly Error InvalidUser = new("Error.InvalidUser", "Invalid Username or password!");
    public static readonly Error Default = new("Error.Default", "Error.Default");
}

//for being immutable i set all properties init!
public class Result
{
    public bool IsSuccess { get; init; }
    public bool IsFailure => !IsSuccess; // For increase readbility
    public Error? Error { get; init; }
    public HttpStatusCode Status { get; init; }
    public string? Message { get; init; }

    protected Result(bool isSuccess, Error? error, HttpStatusCode status = HttpStatusCode.OK, string? message = null)
    {
        if (isSuccess && error != null && error != Error.None)
            throw new InvalidOperationException("Success result must not have an error.");

        if (!isSuccess && (error is null || error == Error.None))
            throw new InvalidOperationException("Failure result must include an error.");

        IsSuccess = isSuccess;
        Error = error;
        Status = status;
        Message = message;
    }

    public static Result Success(HttpStatusCode status = HttpStatusCode.OK, string? message = null) =>
        new(true, Error.None, status, message);

    public static Result Failure(Error error, HttpStatusCode status = HttpStatusCode.InternalServerError, string? message = null) =>
        new(false, error, status, message);

    public static Result<T> Success<T>(T data, HttpStatusCode status = HttpStatusCode.OK, string? message = null) =>
        new(data, true, null, status, message);

    public static Result<T> Failure<T>(Error error, HttpStatusCode status = HttpStatusCode.InternalServerError, string? message = null) =>
        new(default, false, error, status, message);

    public static Result<T> Create<T>(T? value) =>
        value is not null
            ? Success(value)
            : Failure<T>(Error.NullValue);

    public override string ToString() =>
        IsSuccess ? "Success" : $"Failure: {Error?.Code} - {Error?.Message}";
}

public class Result<T> : Result
{
    public T? Data { get; init; }

    internal Result(T? data, bool isSuccess, Error? error, HttpStatusCode status = HttpStatusCode.OK, string? message = null)
        : base(isSuccess, error, status, message)
    {
        Data = data;
    }

    /// <summary>
    /// The Match method in the Result<T> class is inspired by functional programming (especially from languages like F# or Rust), 
    /// and it's used to handle success and failure paths in a clean, declarative way 
    /// — without using if/else 
    /// - or try/catch blocks.
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="onSuccess"></param>
    /// <param name="onFailure"></param>
    /// <returns></returns>
    public TResult Match<TResult>(Func<T, TResult> onSuccess, Func<Error, TResult> onFailure) =>
        IsSuccess ? onSuccess(Data!) : onFailure(Error!);
}

