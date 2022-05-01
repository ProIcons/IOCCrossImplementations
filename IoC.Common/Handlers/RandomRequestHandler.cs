using IoC.Common.Requests;
using IoC.Common.Responses;
using MediatR;

namespace IoC.Common.Handlers;

public class RandomRequestHandler : IRequestHandler<RandomRequest, RandomResponse>
{
    public Task<RandomResponse> Handle(RandomRequest request, CancellationToken cancellationToken)
    {
        if (request.Throw)
        {
            throw new Exception(request.ThrowMessage);
        }

        var rng = new Random();
        return Task.FromResult(new RandomResponse(rng.Next()));
    }
}