using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace WebApi.HealthChecks;

public class LivenessHealthCheck: IHealthCheck
{
    private ILogger<LivenessHealthCheck> _logger;
    public LivenessHealthCheck(ILogger<LivenessHealthCheck> logger)
    {
        _logger = logger;
    }
    
    public Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("LivenessHealthCheck called..");
        return Task.FromResult(
                HealthCheckResult.Healthy("A healthy result."));
    }
}