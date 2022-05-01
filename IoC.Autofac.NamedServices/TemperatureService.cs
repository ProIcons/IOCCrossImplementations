using Autofac.Features.AttributeFilters;
using IoC.Common.Models;
using IoC.Common.Services;

namespace IoC.Autofac.NamedServices;

public class TemperatureService
{
    private readonly IProbeService _namedProbedService;
    private readonly IProbeService _keyedProbedService;

    public TemperatureService(
        [KeyFilter("temperature")] IProbeService namedProbedService,
        [KeyFilter(ProbeType.Temperature)] IProbeService keyedProbedService
    )
    {
        _namedProbedService = namedProbedService;
        _keyedProbedService = keyedProbedService;
    }

    public async Task PrintProbeData()
    {
        Console.WriteLine($"Named Probe Service: {await _namedProbedService.GetProbeMetric()}");
        Console.WriteLine($"Keyed Probe Service: {await _keyedProbedService.GetProbeMetric()}");
    }
}