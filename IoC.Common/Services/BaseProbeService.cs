using IoC.Common.Models;

namespace IoC.Common.Services;

public abstract class BaseProbeService : IProbeService
{
    public abstract Task<ProbeResponse> GetProbeMetric();

    public async Task PrintProbeMetric()
    {
        var result = await GetProbeMetric();
        Console.WriteLine(result);
    }
}