using DryIoc;
using IoC.Common.Extensions;
using IoC.Common.Requests;
using IoC.Interceptors.Common;
using IoC.MS.Interceptors.Extensions;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

var builder = new ServiceCollection();
builder.AddCommonToMicrosoftDependencyInjection();
builder.AddSingleton<CallLogger>();
builder.Intercept(typeof(IRequestHandler<,>), typeof(CallLogger));

var container = builder.BuildServiceProvider();

var mediator = container.GetRequiredService<IMediator>();

// Succeed
var weatherResponse = await mediator.Send(new WeatherDataRequest());
Console.WriteLine(weatherResponse);

// Errored
weatherResponse = await mediator.Send(new WeatherDataRequest(true, "Some Exception"));
Console.WriteLine(weatherResponse);