using DryIoc;
using IoC.Common.Extensions;
using IoC.Common.Requests;
using IoC.DryIoc.Decorators;
using MediatR;

var container = new Container();
container.AddCommonsToDryIoc();
container.Register(typeof(IRequestHandler<,>), typeof(ErrorHandlingDecorator<,>), setup: Setup.Decorator);

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