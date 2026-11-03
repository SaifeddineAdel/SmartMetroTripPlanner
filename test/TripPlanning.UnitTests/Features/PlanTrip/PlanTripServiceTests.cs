using TripPlanning.Domain.Features.PlanTrip.Services;

namespace TripPlanning.UnitTests.Features.PlanTrip
{
    public class PlanTripServiceTests
    {
        private readonly TicketFareCalculator _sut = new();

        [Theory]
        [InlineData(1, 10)]
        [InlineData(9, 10)]
        [InlineData(10, 12)]
        [InlineData(16, 12)]
        [InlineData(17, 15)]
        [InlineData(23, 15)]
        [InlineData(24, 20)]
        [InlineData(39, 20)]
        [InlineData(40, 20)]
        [InlineData(100, 20)]
        public void Calculate_ValidStationCount_ReturnsCorrectFare(
            int stationCount,
            decimal expectedFare)
        {
            // Arrange

            // Act
            var result = _sut.Calculate(stationCount);

            // Assert
            Assert.Equal(expectedFare, result);
        }
    }
}
