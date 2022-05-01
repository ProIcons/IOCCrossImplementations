using Autofac;
using IoC.Autofac.Decorators;
using IoC.Common.Extensions;
using IoC.Common.Requests;
using MediatR;

var builder = new ContainerBuilder();

builder.AddCommonsToAutofac();
builder.RegisterGenericDecorator(typeof(ErrorHandlingDecorator<,>), typeof(IRequestHandler<,>));
var container = builder.Build();

var mediator = container.Resolve<IMediator>();

//Succeed
var weatherResponse = await mediator.Send(new WeatherDataRequest());
Console.WriteLine(weatherResponse);

//Recovered Result
weatherResponse = await mediator.Send(new WeatherDataRequest(true, "Some Exception"));
Console.WriteLine(weatherResponse);

//Recovered Null
var rngResponse = await mediator.Send(new RandomRequest(true, "RNG Failed"));
Console.WriteLine(rngResponse);