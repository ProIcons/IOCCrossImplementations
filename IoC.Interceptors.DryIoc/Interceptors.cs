using DryIoc;
using IoC.Common.Extensions;
using IoC.Common.Requests;
using IoC.Interceptors.Common;
using IoC.Interceptors.DryIoc.Extensions;
using MediatR;

var container = new Container();
container.AddCommonsToDryIoc();
container.Register<CallLogger>(Reuse.Singleton);
container.Intercept(typeof(IRequestHandler<,>), typeof(CallLogger));

var mediator = container.Resolve<IMediator>();

// Succeed
var weatherResponse = await mediator.Send(new WeatherDataRequest());
Console.WriteLine(weatherResponse);

// Errored
weatherResponse = await mediator.Send(new WeatherDataRequest(true, "Some Exception"));
Console.WriteLine(weatherResponse);