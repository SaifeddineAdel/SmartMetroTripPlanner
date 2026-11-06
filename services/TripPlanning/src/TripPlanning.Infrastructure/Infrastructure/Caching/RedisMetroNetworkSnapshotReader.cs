using System.Text.Json;
using StackExchange.Redis;
using TripPlanning.Application.Common.Abstractions;
using TripPlanning.Application.Common.DTOs;

namespace TripPlanning.Infrastructure.Caching;

public sealed class RedisMetroNetworkSnapshotReader
    : IMetroNetworkSnapshotReader
{
    private const string SnapshotKey = "metro:network:snapshot";

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    private readonly IConnectionMultiplexer _redis;

    public RedisMetroNetworkSnapshotReader(
        IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public async Task<MetroNetworkSnapshot?> GetSnapshotAsync(
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        var database = _redis.GetDatabase();

        var snapshotValue = await database.StringGetAsync(
            SnapshotKey);

        if (!snapshotValue.HasValue)
        {
            return null;
        }

        var snapshot = JsonSerializer.Deserialize<MetroNetworkSnapshot>(
            snapshotValue.ToString(),
            JsonOptions);

        if (snapshot is null)
        {
            throw new InvalidOperationException(
                "Redis returned an invalid metro network snapshot.");
        }

        return snapshot;
    }
}