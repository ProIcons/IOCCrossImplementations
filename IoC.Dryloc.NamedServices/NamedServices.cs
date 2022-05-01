using DryIoc;
using IoC.Common.Extensions;
using IoC.Common.Models;
using IoC.Common.Services;
using IoC.DryIoc.NamedServices;

var container = new Container();

container.AddCommonsToDryIoc();
container.Register<IProbeService, HumidityProbeService>(serviceKey: ProbeType.Humidity);
container.Register<IProbeService, HumidityProbeService>(serviceKey: "humidity");
container.Register<IProbeService, TemperatureProbeService>(serviceKey: ProbeType.Temperature);
container.Register<IProbeService, TemperatureProbeService>(serviceKey: "temperature");

container.Register<TemperatureService>();
container.Register<HumidityService>();

await container.Resolve<IProbeService>("humidity").PrintProbeMetric();
await container.Resolve<IProbeService>(ProbeType.Humidity).PrintProbeMetric();
await container.Resolve<HumidityService>().PrintProbeData();

await container.Resolve<IProbeService>("temperature").PrintProbeMetric();
await container.Resolve<IProbeService>(ProbeType.Temperature).PrintProbeMetric();
await container.Resolve<TemperatureService>().PrintProbeData();