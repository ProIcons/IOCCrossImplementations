using IoC.Common.Requests;
using IoC.Common.Responses;
using MediatR;

namespace IoC.Common.Handlers;

public class WeatherDataRequestHandler : IRequestHandler<WeatherDataRequest, Response<WeatherDataResponse>>
{
    private static readonly string[] Summaries =
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };


    public Task<Response<WeatherDataResponse>> Handle(WeatherDataRequest request, CancellationToken cancellationToken)
    {
        if (request.Throw)
        {
            throw new Exception(request.ThrowMessage);
        }

        var rng = new Random();
        var response = new WeatherDataResponse
        {
            Date = DateTime.Now,
            TemperatureC = rng.Next(-20, 55),
            Summary = Summaries[rng.Next(Summaries.Length)]
        };

        return Task.FromResult<Response<WeatherDataResponse>>(response);
    }
}