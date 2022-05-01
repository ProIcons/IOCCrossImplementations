using Autofac;
using Autofac.Extras.DynamicProxy;
using IoC.Common.Extensions;
using IoC.Common.Handlers;
using IoC.Common.Requests;
using IoC.Common.Responses;
using MediatR;

var builder = new ContainerBuilder();

builder.AddCommonsToAutofac();
builder.RegisterType<CallLogger>();
builder.RegisterType<WeatherDataRequestHandler>()
    .As<IRequestHandler<WeatherDataRequest, Response<WeatherDataResponse>>>()
    .EnableInterfaceInterceptors()
    .InterceptedBy(typeof(CallLogger));

var container = builder.Build();

var mediator = container.Resolve<IMediator>();

// Succeed
var weatherResponse = await mediator.Send(new WeatherDataRequest());
Console.WriteLine(weatherResponse);

// Errored
weatherResponse = await mediator.Send(new WeatherDataRequest(true, "Some Exception"));
Console.WriteLine(weatherResponse);