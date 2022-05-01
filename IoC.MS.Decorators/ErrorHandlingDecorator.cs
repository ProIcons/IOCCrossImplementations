using IoC.Common.Responses;
using MediatR;

namespace IoC.MS.Decorators;

public interface IErrorHandlingDecorator
{
}

public class ErrorHandlingDecorator<TRequest, TResponse> : IErrorHandlingDecorator, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : class
{
    private readonly IRequestHandler<TRequest, TResponse> _decorated;

    public ErrorHandlingDecorator(IRequestHandler<TRequest, TResponse> decorated)
    {
        this._decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
    }

    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        try
        {
            return await _decorated.Handle(request, cancellationToken);
        }
        catch (Exception e)
        {
            Type[]? genericTypes = typeof(TResponse).GetGenericArguments();
            var constructedType = genericTypes is not null && genericTypes.Length > 0
                ? typeof(Response<>).MakeGenericType(genericTypes[0])
                : null;

            if (constructedType is not null && constructedType.IsAssignableTo(typeof(TResponse)))
            {
                return (TResponse)Activator.CreateInstance(typeof(TResponse), e);
            }

            Console.WriteLine($"Recovered Error on an Unrecognized Return Type '{typeof(TResponse).Name}': {e.Message}");
            return null!;
        }
    }
}