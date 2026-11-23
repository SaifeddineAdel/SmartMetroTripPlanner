using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using MetroNetwork.Application.Common.Abstractions;
using MetroNetwork.Application.Common.DTOs;
using MetroNetwork.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace MetroNetwork.Infrastructure.Caching;

public sealed class RedisMetroNetworkSnapshotPublisher
    : IMetroNetworkSnapshotPublisher
{
    private const string SnapshotKey = "metro:network:snapshot";

    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web);

    private readonly MetroNetworkDbContext _context;
    private readonly IConnectionMultiplexer _redis;
    private readonly ILogger<RedisMetroNetworkSnapshotPublisher> _logger;

    public RedisMetroNetworkSnapshotPublisher(
        MetroNetworkDbContext context,
        IConnectionMultiplexer redis,
        ILogger<RedisMetroNetworkSnapshotPublisher> logger)
    {
        _context = context;
        _redis = redis;
        _logger = logger;
    }

    public async Task<bool> PublishIfChangedAsync(
        CancellationToken cancellationToken = default)
    {
        var stations = await _context.Stations
            .AsNoTracking()
            .OrderBy(station => station.Id)
            .Select(station => new StationSnapshot(
                station.Id,
                station.Name))
            .ToListAsync(cancellationToken);

        var activeStationIds = stations
            .Select(station => station.Id)
            .ToList();

        var metroLines = await _context.MetroLines
            .AsNoTracking()
            .OrderBy(line => line.Id)
            .Select(line => new MetroLineSnapshot(
                line.Id,
                line.Name))
            .ToListAsync(cancellationToken);

        var lineStations = await _context.LineStations
            .AsNoTracking()
            .Where(lineStation =>
                activeStationIds.Contains(lineStation.StationId))
            .OrderBy(lineStation => lineStation.MetroLineId)
            .ThenBy(lineStation => lineStation.Sequence)
            .Select(lineStation => new LineStationSnapshot(
                lineStation.MetroLineId,
                lineStation.StationId,
                lineStation.Sequence))
            .ToListAsync(cancellationToken);

        var content = new
        {
            Stations = stations,
            MetroLines = metroLines,
            LineStations = lineStations
        };

        var contentJson = JsonSerializer.Serialize(
            content,
            JsonOptions);

        var contentBytes = Encoding.UTF8.GetBytes(contentJson);

        var hashBytes = SHA256.HashData(contentBytes);

        var version = Convert.ToHexString(hashBytes);

        var snapshot = new MetroNetworkSnapshot
        {
            Version = version,
            PublishedAtUtc = DateTimeOffset.UtcNow,
            Stations = stations,
            MetroLines = metroLines,
            LineStations = lineStations
        };

        try
        {
            var database = _redis.GetDatabase();

            var existingValue = await database.StringGetAsync(
                SnapshotKey);

            if (existingValue.HasValue)
            {
                try
                {
                    var existingSnapshot =
                        JsonSerializer.Deserialize<MetroNetworkSnapshot>(
                            existingValue.ToString(),
                            JsonOptions);

                    if (existingSnapshot?.Version == version)
                    {
                        return false;
                    }
                }
                catch (JsonException)
                {
                    _logger.LogWarning(
                        "Existing Redis snapshot is invalid. It will be replaced.");
                }
            }

            var snapshotJson = JsonSerializer.Serialize(
                snapshot,
                JsonOptions);

            await database.StringSetAsync(
                SnapshotKey,
                snapshotJson);

            _logger.LogInformation(
                "Metro network snapshot published. Version: {Version}",
                version);

            return true;
        }
        catch (RedisException exception)
        {
            _logger.LogWarning(
                exception,
                "Could not publish metro network snapshot to Redis.");

            return false;
        }
    }
}