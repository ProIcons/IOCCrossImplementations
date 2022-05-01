using IoC.Common.Models;

namespace IoC.Common.Services;

public interface IProbeService
{
    Task<ProbeResponse> GetProbeMetric();
    Task PrintProbeMetric();
}