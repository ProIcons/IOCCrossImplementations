using IoC.Common.Responses;
using MediatR;

namespace IoC.Common.Requests;

public record WeatherDataRequest(bool Throw = false, string ThrowMessage = "Error") : IRequest<Response<WeatherDataResponse>>;