
using Microsoft.AspNetCore.Mvc;
using MetroNetwork.Application.Features.Stations;
using MetroNetwork.Application.Features.Stations.DTOs;

namespace MetroNetwork.Api.Features.Stations;

[ApiController]
[Route("api/stations")]
public sealed class StationsController : ControllerBase
{
    private readonly IStationService _stationService;

    public StationsController(IStationService stationService)
    {
        _stationService = stationService;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(StationResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<StationResponse>> Create(
        [FromBody] CreateStationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _stationService.CreateAsync(
            request.Name,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(
        typeof(StationResponse),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<StationResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _stationService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<StationResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<StationResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var response = await _stationService.GetAllAsync(
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Rename(
        int id,
        [FromBody] RenameStationRequest request,
        CancellationToken cancellationToken)
    {
        await _stationService.RenameAsync(
            id,
            request.Name,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _stationService.DeleteAsync(
            id,
            cancellationToken);

        return NoContent();
    }
}
