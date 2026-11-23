using TripPlanning.Application.Features.PlanTrip.Services;

namespace TripPlanning.Api.BackgroundServices;

public sealed class MetroNetworkSyncService : BackgroundService
{
    private readonly MetroNetworkStateProvider _stateProvider;
    private readonly ILogger<MetroNetworkSyncService> _logger;

    public MetroNetworkSyncService(
        MetroNetworkStateProvider stateProvider,
        ILogger<MetroNetworkSyncService> logger)
    {
        _stateProvider = stateProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var updated = await _stateProvider.RefreshAsync(
                    stoppingToken);

                if (updated)
                {
                    var state = _stateProvider.GetCurrentState();

                    _logger.LogInformation(
                        "Metro network Graph updated. Version: {Version}",
                        state.Version);
                }
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Failed to synchronize metro network from Redis.");
            }

            try
            {
                await Task.Delay(
                    TimeSpan.FromSeconds(5),
                    stoppingToken);
            }
            catch (OperationCanceledException)
                when (stoppingToken.IsCancellationRequested)
            {
                break;
            }
        }
    }
}