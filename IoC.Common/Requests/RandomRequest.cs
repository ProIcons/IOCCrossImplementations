using IoC.Common.Responses;
using MediatR;

namespace IoC.Common.Requests;

public record RandomRequest(bool Throw = false, string ThrowMessage = "Error") : IRequest<RandomResponse>;