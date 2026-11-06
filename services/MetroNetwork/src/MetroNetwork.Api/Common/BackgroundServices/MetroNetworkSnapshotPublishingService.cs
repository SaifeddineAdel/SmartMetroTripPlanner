using MetroNetwork.Application.Common.Abstractions;

namespace MetroNetwork.Api.Common.BackgroundServices;

public sealed class MetroNetworkSnapshotPublishingService
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<MetroNetworkSnapshotPublishingService> _logger;

    public MetroNetworkSnapshotPublishingService(
        IServiceScopeFactory scopeFactory,
        ILogger<MetroNetworkSnapshotPublishingService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();

                var publisher = scope.ServiceProvider
                    .GetRequiredService<IMetroNetworkSnapshotPublisher>();

                await publisher.PublishIfChangedAsync(stoppingToken);
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
                    "An error occurred while publishing metro network snapshot.");
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