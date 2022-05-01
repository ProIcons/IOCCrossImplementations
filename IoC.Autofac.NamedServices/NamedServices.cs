using Autofac;
using Autofac.Features.AttributeFilters;
using Autofac.Features.Indexed;
using IoC.Autofac.NamedServices;
using IoC.Common.Extensions;
using IoC.Common.Models;
using IoC.Common.Services;

var builder = new ContainerBuilder();

builder.AddCommonsToAutofac();
builder.RegisterType<HumidityProbeService>()
    .Named<IProbeService>("humidity")
    .Keyed<IProbeService>(ProbeType.Humidity);
builder.RegisterType<TemperatureProbeService>()
    .Named<IProbeService>("temperature")
    .Keyed<IProbeService>(ProbeType.Temperature);

builder.RegisterType<TemperatureService>()
    .WithAttributeFiltering();
builder.RegisterType<HumidityService>()
    .WithAttributeFiltering();

var container = builder.Build();

await container.ResolveNamed<IProbeService>("humidity").PrintProbeMetric();
await container.ResolveKeyed<IProbeService>(ProbeType.Humidity).PrintProbeMetric();
await container.Resolve<IIndex<string, IProbeService>>()["humidity"].PrintProbeMetric();
await container.Resolve<IIndex<ProbeType, IProbeService>>()[ProbeType.Humidity].PrintProbeMetric();
await container.Resolve<HumidityService>().PrintProbeData();

await container.ResolveNamed<IProbeService>("temperature").PrintProbeMetric();
await container.ResolveKeyed<IProbeService>(ProbeType.Temperature).PrintProbeMetric();
await container.Resolve<IIndex<string, IProbeService>>()["temperature"].PrintProbeMetric();
await container.Resolve<IIndex<ProbeType, IProbeService>>()[ProbeType.Temperature].PrintProbeMetric();
await container.Resolve<TemperatureService>().PrintProbeData();