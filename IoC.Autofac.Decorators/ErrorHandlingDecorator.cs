using Autofac.Features.Decorators;
using IoC.Common.Responses;
using MediatR;

namespace IoC.Autofac.Decorators;

public interface IErrorHandlingDecorator
{
}

public class ErrorHandlingDecorator<TRequest, TResponse> : IErrorHandlingDecorator, IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse> where TResponse : class
{
    private readonly IRequestHandler<TRequest, TResponse> _decorated;
    private readonly IDecoratorContext _context;

    public ErrorHandlingDecorator(IRequestHandler<TRequest, TResponse> decorated, IDecoratorContext context)
    {
        this._decorated = decorated ?? throw new ArgumentNullException(nameof(decorated));
        this._context = context ?? throw new ArgumentNullException(nameof(context));
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