using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TripPlanning.Application.Features.PlanTrip.DTOs;
using TripPlanning.Application.Features.PlanTrip.Services;

namespace TripPlanning.Api.Controllers
{
    [Route("api/trips")]
    [ApiController]
    public class PlanTripsController : ControllerBase
    {
        private readonly PlanTripService _planTripService;

        public PlanTripsController(PlanTripService planTripService)
        {
            _planTripService = planTripService;
        }

        [HttpPost("plan")]
        public async Task<ActionResult<TripPlanResponse>> Plan(
            [FromBody] PlanTripRequest request,
            CancellationToken cancellationToken)
        {
            var response = await _planTripService.PlanAsync(
                request,
                cancellationToken);

            return Ok(response);
        }
    }
}
