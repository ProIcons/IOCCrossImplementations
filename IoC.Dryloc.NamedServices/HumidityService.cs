using IoC.Common.Models;
using IoC.Common.Services;

namespace IoC.DryIoc.NamedServices;

public class HumidityService
{
    private readonly IProbeService _namedProbedService;
    private readonly IProbeService _keyedProbedService;

    public HumidityService(
        KeyValuePair<object, IProbeService>[] probeServiceResolver
    )
    {
        _namedProbedService = probeServiceResolver.First(x => Equals(x.Key, "humidity")).Value;
        _keyedProbedService = probeServiceResolver.First(x => Equals(x.Key, ProbeType.Humidity)).Value;;
    }

    public async Task PrintProbeData()
    {
        Console.WriteLine($"Named Probe Service: {await _namedProbedService.GetProbeMetric()}");
        Console.WriteLine($"Keyed Probe Service: {await _keyedProbedService.GetProbeMetric()}");
    }
}