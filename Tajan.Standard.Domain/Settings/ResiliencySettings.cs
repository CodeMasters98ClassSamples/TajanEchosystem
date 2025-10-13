namespace Tajan.Standard.Domain.Settings;

public class ResiliencySettings
{
    public int TimeoutInMilliseconds { get; set; }
    public int RetryCount { get; set; }
    public int SamplingDurationToCheckForFailureInSeconds { get; set; }
    public float FailureRatioToBreakTheCircuit { get; set; }
    public int MinimumThroughputToCheckForFailure { get; set; }
    public int BreakDurationInSeconds { get; set; }
}
