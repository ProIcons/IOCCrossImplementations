using IoC.Common.Models;

namespace IoC.Common.Services;

public class HumidityProbeService : BaseProbeService
{
    public override Task<ProbeResponse> GetProbeMetric()
    {
        var rng = new Random();
        return Task.FromResult(
            new ProbeResponse(DateTime.Now, rng.NextDouble(), ProbeType.Humidity)
        );
    }
}