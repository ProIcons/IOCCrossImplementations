using IoC.Common.Models;

namespace IoC.Common.Services;

public class TemperatureProbeService : BaseProbeService
{
    public override Task<ProbeResponse> GetProbeMetric()
    {
        var rng = new Random();
        return Task.FromResult(
            new ProbeResponse(DateTime.Now, rng.Next(-55, 55), ProbeType.Temperature)
        );
    }
}