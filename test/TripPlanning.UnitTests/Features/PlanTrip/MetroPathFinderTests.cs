using TripPlanning.Domain.Features.PlanTrip.Graph;
using TripPlanning.Domain.Features.PlanTrip.Services;

namespace TripPlanning.UnitTests.Features.PlanTrip
{
    public class MetroPathFinderTests
    {
        private readonly MetroPathFinder _sut = new();

        [Fact]
        public void FindShortestPath_SameStartAndDestination_ReturnsEmptyPath()
        {
            // Arrange
            var graph = new MetroGraph();

            // Act
            var result = _sut.FindShortestPath(graph, 1, 1);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Fact]
        public void FindShortestPath_NoAvailableRoute_ReturnNull()
        {
            // Arrange
            var graph = new MetroGraph();

            graph.AddConnection(new GraphConnection(1, 2, 1));

            // Act
            var result = _sut.FindShortestPath(graph, 1, 3);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void FindShortestPath_DirectRouteExists_ReturnsDirectRoute()
        {
            // Arrange
            var graph = new MetroGraph();

            // Direct route 1 -> 3
            graph.AddConnection(new GraphConnection(1, 3, 1));

            // Alternate route 1 -> 2 -> 3
            graph.AddConnection(new GraphConnection(1, 2, 1));
            graph.AddConnection(new GraphConnection(2, 3, 1));

            // Act
            var result = _sut.FindShortestPath(graph, 1, 3);

            // Assert
            Assert.NotNull(result);

            Assert.Single(result);

            Assert.Equal(new GraphConnection(1, 3, 1), result[0]);
        }

        [Fact]
        public void FindShortestPath_MultipleRoutesExist_ReturnsRouteWithMinimumStations()
        {
            // Arrange
            var graph = new MetroGraph();

            // Route 1: 1 -> 2 -> 4 (2 connections, 1 transfer)
            graph.AddConnection(new GraphConnection(1, 2, 1));
            graph.AddConnection(new GraphConnection(2, 4, 2));

            // Route 2: 1 -> 3 -> 5 -> 4 (3 connections, no transfer) 
            graph.AddConnection(new GraphConnection(1, 3, 3));
            graph.AddConnection(new GraphConnection(3, 5, 3));
            graph.AddConnection(new GraphConnection(5, 4, 3));

            // Act
            var result = _sut.FindShortestPath(graph, 1, 4);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(new[]
            {
                new GraphConnection(1, 2, 1),
                new GraphConnection(2, 4, 2)
            },
            result);
        }

        [Fact]
        public void FindShortestPath_StationCountIsEqual_ReturnsRouteWithMinimumTransfers()
        {
            // Arrange
            var graph = new MetroGraph();

            //Route 1: 1 -> 2 -> 4 (1 transfer)
            graph.AddConnection(new GraphConnection(1, 2, 1));
            graph.AddConnection(new GraphConnection(2, 4, 2));

            //Route 2: 1 -> 3 -> 4 (0 transfer)
            graph.AddConnection(new GraphConnection(1, 3, 3));
            graph.AddConnection(new GraphConnection(3, 4, 3));

            // Act
            var result = _sut.FindShortestPath(graph, 1, 4);

            // Assert
            Assert.NotNull(result);

            Assert.Equal(new[]
            {
                new GraphConnection(1, 3, 3),
                new GraphConnection(3, 4, 3)
            },
            result);
        }
    }
}
