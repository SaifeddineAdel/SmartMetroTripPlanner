namespace MetroNetwork.Application.Common.Abstractions;

public interface IMetroNetworkSnapshotPublisher
{
    Task<bool> PublishIfChangedAsync(
        CancellationToken cancellationToken = default);
}