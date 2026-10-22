using System.Net.Http.Json;
using TripPlanning.Application.Common.Exceptions;
using TripPlanning.Application.Features.PlanTrip.Abstractions;
using TripPlanning.Application.Features.PlanTrip.DTOs;

namespace TripPlanning.Infrastructure.Clients;

public sealed class MetroNetworkClient : IMetroNetworkClient
{
    private readonly HttpClient _httpClient;

    public MetroNetworkClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<StationData>> GetStationsAsync(
        CancellationToken cancellationToken)
    {
        var stations = await _httpClient.GetFromJsonAsync<List<StationData>>(
            "api/stations",
            cancellationToken);

        return stations
            ?? throw new ServiceUnavailableException(
                "MetroNetwork returned a null stations response.");
    }

    public async Task<IReadOnlyList<MetroLineData>> GetMetroLinesAsync(
        CancellationToken cancellationToken)
    {
        var lines = await _httpClient.GetFromJsonAsync<List<MetroLineData>>(
            "api/metro-lines",
            cancellationToken);

        return lines
            ?? throw new ServiceUnavailableException(
                "MetroNetwork returned a null metro lines response.");
    }

    public async Task<IReadOnlyList<LineStationData>> GetLineStationsAsync(
        int metroLineId,
        CancellationToken cancellationToken)
    {
        var stations = await _httpClient.GetFromJsonAsync<List<LineStationData>>(
            $"api/metro-lines/{metroLineId}/stations",
            cancellationToken);

        return stations
            ?? throw new ServiceUnavailableException(
                $"MetroNetwork returned a null response for line {metroLineId}.");
    }
}