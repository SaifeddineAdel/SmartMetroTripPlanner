
using Microsoft.AspNetCore.Mvc;
using MetroNetwork.Application.Features.LineStations;
using MetroNetwork.Application.Features.LineStations.DTOs;

namespace MetroNetwork.Api.Features.LineStations;

[ApiController]
[Route("api/metro-lines/{metroLineId:int}/stations")]
public sealed class LineStationsController : ControllerBase
{
    private readonly ILineStationService _service;

    public LineStationsController(ILineStationService service)
    {
        _service = service;
    }

    [HttpPost]
    [ProducesResponseType(
        typeof(LineStationResponse),
        StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<LineStationResponse>> AddStation(
        int metroLineId,
        [FromBody] CreateLineStationRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _service.AddStationToLineAsync(
            metroLineId,
            request,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetStations),
            new { metroLineId },
            response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<LineStationResponse>),
        StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<LineStationResponse>>> GetStations(
        int metroLineId,
        CancellationToken cancellationToken)
    {
        var response = await _service.GetStationsByLineAsync(
            metroLineId,
            cancellationToken);

        return Ok(response);
    }

    [HttpPut("{stationId:int}/sequence")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeSequence(
        int metroLineId,
        int stationId,
        [FromBody] ChangeLineStationSequenceRequest request,
        CancellationToken cancellationToken)
    {
        await _service.ChangeSequenceAsync(
            metroLineId,
            stationId,
            request.Sequence,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{stationId:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemoveStation(
        int metroLineId,
        int stationId,
        CancellationToken cancellationToken)
    {
        await _service.RemoveStationFromLineAsync(
            metroLineId,
            stationId,
            cancellationToken);

        return NoContent();
    }
}
