namespace IoC.Common.Responses;

public record Response<TPayload>
{
    public Response()
    {
    }

    public Response(TPayload? result)
    {
        Result = result;
        IsErrored = false;
    }

    public Response(Exception? exception)
    {
        Exception = exception;
        IsErrored = true;
    }

    public static Response<TPayload> OfSuccess<TPayload>(TPayload payload) =>
        new(payload);

    public static Response<TPayload> OfFailure<TPayload>(Exception exc) =>
        new(exc);

    public TPayload? Result { get; init; }
    public bool IsErrored { get; init; }
    public Exception? Exception { get; init; }

    public static implicit operator Response<TPayload>(TPayload payload) => OfSuccess<TPayload>(payload);
    public static implicit operator Response<TPayload>(Exception exception) => OfFailure<TPayload>(exception);
}