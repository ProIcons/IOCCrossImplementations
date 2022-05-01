namespace IoC.Common.Models;

public record ProbeResponse(DateTime MetricDateTime, double Metric, ProbeType Type);