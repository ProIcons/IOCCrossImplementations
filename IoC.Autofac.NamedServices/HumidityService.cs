using Autofac.Features.AttributeFilters;
using IoC.Common.Models;
using IoC.Common.Services;

namespace IoC.Autofac.NamedServices;

public class HumidityService
{
    private readonly IProbeService _namedProbedService;
    private readonly IProbeService _keyedProbedService;

    public HumidityService(
        [KeyFilter("humidity")] IProbeService namedProbedService,
        [KeyFilter(ProbeType.Humidity)] IProbeService keyedProbedService
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