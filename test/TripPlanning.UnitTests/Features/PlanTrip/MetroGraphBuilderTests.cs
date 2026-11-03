using Moq;
using TripPlanning.Application.Features.PlanTrip.Abstractions;
using TripPlanning.Application.Features.PlanTrip.DTOs;
using TripPlanning.Application.Features.PlanTrip.Services;
using TripPlanning.Domain.Features.PlanTrip.Graph;


namespace TripPlanning.UnitTests.Features.PlanTrip
{
    public class MetroGraphBuilderTests
    {
        private readonly Mock<IMetroNetworkClient> _clientMock;
        private readonly MetroGraphBuilder _sut;

        public MetroGraphBuilderTests()
        {
            _clientMock = new Mock<IMetroNetworkClient>();
            _sut = new MetroGraphBuilder(_clientMock.Object);
        }

        [Fact]
        public async Task BuildAsync_NoMetroLines_ResturnEmptyGraph()
        {
            // Arrang
            IReadOnlyList<MetroLineData> lines = [];

            _clientMock
                .Setup(client => client.GetMetroLinesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lines);

            // Act
            var result = await _sut.BuildAsync(CancellationToken.None);

            // Assert
            Assert.Empty(result.Connections);
        }

        [Fact]
        public async Task BuildAsync_TwoStationsExists_CreatesBidirectionalConnections()
        {
            // Arrange
            IReadOnlyList<MetroLineData> lines =
                [
                    new MetroLineData(1, "Line 1")
                ];

            IReadOnlyList<LineStationData> lineStations =
                [
                    new LineStationData(1, 10, 1),
                    new LineStationData(1, 20, 2)
                ];

            _clientMock
                .Setup(client => client.GetMetroLinesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lines);

            _clientMock
                .Setup(client => client.GetLineStationsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lineStations);

            // Act
            var result = await _sut.BuildAsync(CancellationToken.None);

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
        public async Task BuildAsync_StationsAreUnorderd_CreatesConnectionsBasedOnSequence()
        {
            // Arrange
            IReadOnlyList<MetroLineData> lines =
            [
                new MetroLineData(1, "Line 1")
            ];

            IReadOnlyList<LineStationData> lineStations =
            [
                new LineStationData(1, 30, 3),
                new LineStationData(1, 10, 1),
                new LineStationData(1, 20, 2)
            ];

            _clientMock
                .Setup(client => client.GetMetroLinesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lines);

            _clientMock
                .Setup(client => client.GetLineStationsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lineStations);

            // Act
            var result = await _sut.BuildAsync(CancellationToken.None);

            // Assert
            Assert.Equal(4, result.Connections.Count);

            Assert.Contains(new GraphConnection(10, 20, 1), result.Connections);
            Assert.Contains(new GraphConnection(20, 30, 1), result.Connections);

            Assert.Contains(new GraphConnection(30, 20, 1), result.Connections);
            Assert.Contains(new GraphConnection(20, 10, 1), result.Connections);
        }

        [Fact]
        public async Task BuildAsync_MultipleLinesExist_BuildsConnectionsForEachLine()
        {
            // Arrange
            IReadOnlyList<MetroLineData> lines =
            [
                new MetroLineData(1, "Line 1"),
                new MetroLineData(2, "Line 2")
            ];

            IReadOnlyList<LineStationData> line1Stations =
            [
                new LineStationData(1, 10, 1),
                new LineStationData(1, 20, 2)
            ];

            IReadOnlyList<LineStationData> line2Stations =
            [
                new LineStationData(2, 20, 1),
                new LineStationData(2, 30, 2)
            ];

            _clientMock
                .Setup(client => client.GetMetroLinesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lines);

            _clientMock
                .Setup(client => client.GetLineStationsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(line1Stations);

            _clientMock
                .Setup(client => client.GetLineStationsAsync(
                    2,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(line2Stations);

            // Act
            var result = await _sut.BuildAsync(CancellationToken.None);

            // Assert
            Assert.Equal(4, result.Connections.Count);

            Assert.Contains(new GraphConnection(10, 20, 1), result.Connections);
            Assert.Contains(new GraphConnection(20, 10, 1), result.Connections);

            Assert.Contains(new GraphConnection(20, 30, 2), result.Connections);
            Assert.Contains(new GraphConnection(30, 20, 2), result.Connections);
        }

        [Fact]
        public async Task BuildAsync_LineHasOneStation_ReturnsEmptyGraph()
        {
            // Arrange
            IReadOnlyList<MetroLineData> lines =
            [
                new MetroLineData(1, "Line 1")
            ];

            IReadOnlyList<LineStationData> lineStations =
            [
                new LineStationData(1, 10, 1)
            ];

            _clientMock
                .Setup(client => client.GetMetroLinesAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lines);

            _clientMock
                .Setup(client => client.GetLineStationsAsync(
                    1,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(lineStations);

            // Act
            var result = await _sut.BuildAsync(CancellationToken.None);

            // Assert
            Assert.Empty(result.Connections);
        }
    }
}
