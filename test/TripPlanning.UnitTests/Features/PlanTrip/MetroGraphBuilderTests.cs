using TripPlanning.Application.Common.DTOs;
using TripPlanning.Application.Features.PlanTrip.Services;
using TripPlanning.Domain.Features.PlanTrip.Graph;

namespace TripPlanning.UnitTests.Features.PlanTrip;

public class MetroGraphBuilderTests
{
    private readonly MetroGraphBuilder _sut;

    public MetroGraphBuilderTests()
    {
        _sut = new MetroGraphBuilder();
    }

    [Fact]
    public void Build_NoMetroLines_ReturnsEmptyGraph()
    {
        // Arrange
        var snapshot = new MetroNetworkSnapshot();

        // Act
        var result = _sut.Build(snapshot);

        // Assert
        Assert.Empty(result.Connections);
    }

    [Fact]
    public void Build_TwoStationsExist_CreatesBidirectionalConnections()
    {
        // Arrange
        var snapshot = new MetroNetworkSnapshot
        {
            MetroLines =
            [
                new MetroLineSnapshot(1, "Line 1")
            ],

            Stations =
            [
                new StationSnapshot(10, "Station 10"),
                new StationSnapshot(20, "Station 20")
            ],

            LineStations =
            [
                new LineStationSnapshot(1, 10, 1),
                new LineStationSnapshot(1, 20, 2)
            ]
        };

        // Act
        var result = _sut.Build(snapshot);

        // Assert
        Assert.Equal(2, result.Connections.Count);

        Assert.Contains(
            new GraphConnection(10, 20, 1),
            result.Connections);

        Assert.Contains(
            new GraphConnection(20, 10, 1),
            result.Connections);
    }

    [Fact]
    public void Build_StationsAreUnordered_CreatesConnectionsBasedOnSequence()
    {
        // Arrange
        var snapshot = new MetroNetworkSnapshot
        {
            MetroLines =
            [
                new MetroLineSnapshot(1, "Line 1")
            ],

            Stations =
            [
                new StationSnapshot(10, "Station 10"),
                new StationSnapshot(20, "Station 20"),
                new StationSnapshot(30, "Station 30")
            ],

            LineStations =
            [
                new LineStationSnapshot(1, 30, 3),
                new LineStationSnapshot(1, 10, 1),
                new LineStationSnapshot(1, 20, 2)
            ]
        };

        // Act
        var result = _sut.Build(snapshot);

        // Assert
        Assert.Equal(4, result.Connections.Count);

        Assert.Contains(
            new GraphConnection(10, 20, 1),
            result.Connections);

        Assert.Contains(
            new GraphConnection(20, 30, 1),
            result.Connections);

        Assert.Contains(
            new GraphConnection(30, 20, 1),
            result.Connections);

        Assert.Contains(
            new GraphConnection(20, 10, 1),
            result.Connections);
    }

    [Fact]
    public void Build_MultipleLinesExist_BuildsConnectionsForEachLine()
    {
        // Arrange
        var snapshot = new MetroNetworkSnapshot
        {
            MetroLines =
            [
                new MetroLineSnapshot(1, "Line 1"),
                new MetroLineSnapshot(2, "Line 2")
            ],

            Stations =
            [
                new StationSnapshot(10, "Station 10"),
                new StationSnapshot(20, "Station 20"),
                new StationSnapshot(30, "Station 30")
            ],

            LineStations =
            [
                new LineStationSnapshot(1, 10, 1),
                new LineStationSnapshot(1, 20, 2),

                new LineStationSnapshot(2, 20, 1),
                new LineStationSnapshot(2, 30, 2)
            ]
        };

        // Act
        var result = _sut.Build(snapshot);

        // Assert
        Assert.Equal(4, result.Connections.Count);

        Assert.Contains(
            new GraphConnection(10, 20, 1),
            result.Connections);

        Assert.Contains(
            new GraphConnection(20, 10, 1),
            result.Connections);

        Assert.Contains(
            new GraphConnection(20, 30, 2),
            result.Connections);

        Assert.Contains(
            new GraphConnection(30, 20, 2),
            result.Connections);
    }

    [Fact]
    public void Build_LineHasOneStation_ReturnsEmptyGraph()
    {
        // Arrange
        var snapshot = new MetroNetworkSnapshot
        {
            MetroLines =
            [
                new MetroLineSnapshot(1, "Line 1")
            ],

            Stations =
            [
                new StationSnapshot(10, "Station 10")
            ],

            LineStations =
            [
                new LineStationSnapshot(1, 10, 1)
            ]
        };

        // Act
        var result = _sut.Build(snapshot);

        // Assert
        Assert.Empty(result.Connections);
    }
}