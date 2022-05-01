using IoC.Common.Extensions;
using IoC.Common.Requests;
using IoC.MS.Decorators;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

IServiceCollection collection = new ServiceCollection();
collection.AddCommonToMicrosoftDependencyInjection();
collection.Decorate(typeof(IRequestHandler<,>), typeof(ErrorHandlingDecorator<,>));

var container = collection.BuildServiceProvider();

var mediator = container.GetRequiredService<IMediator>();

//Succeed
var weatherResponse = await mediator.Send(new WeatherDataRequest());
Console.WriteLine(weatherResponse);

//Recovered Result
weatherResponse = await mediator.Send(new WeatherDataRequest(true, "Some Exception"));
Console.WriteLine(weatherResponse);

//Recovered Null
var rngResponse = await mediator.Send(new RandomRequest(true, "RNG Failed"));
Console.WriteLine(rngResponse);
