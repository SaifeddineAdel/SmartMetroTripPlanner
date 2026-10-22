using Microsoft.AspNetCore.Mvc;
using MetroNetwork.Application.Features.MetroLines;
using MetroNetwork.Application.Features.MetroLines.DTOs;

namespace MetroNetwork.Api.Features.MetroLines;

[ApiController]
[Route("api/metro-lines")]
public sealed class MetroLinesController : ControllerBase
{
    private readonly IMetroLineService _metroLineService;

    public MetroLinesController(IMetroLineService metroLineService)
    {
        _metroLineService = metroLineService;
    }

    [HttpPost]
    [ProducesResponseType(typeof(MetroLineResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<MetroLineResponse>> Create(
        [FromBody] CreateMetroLineRequest request,
        CancellationToken cancellationToken)
    {
        var response = await _metroLineService.CreateAsync(
            request.Name,
            cancellationToken);

        return CreatedAtAction(
            nameof(GetById),
            new { id = response.Id },
            response);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MetroLineResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MetroLineResponse>> GetById(
        int id,
        CancellationToken cancellationToken)
    {
        var response = await _metroLineService.GetByIdAsync(
            id,
            cancellationToken);

        return Ok(response);
    }

    [HttpGet]
    [ProducesResponseType(
        typeof(IReadOnlyList<MetroLineResponse>),
        StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MetroLineResponse>>> GetAll(
        CancellationToken cancellationToken)
    {
        var response = await _metroLineService.GetAllAsync(cancellationToken);

        return Ok(response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Rename(
        int id,
        [FromBody] RenameMetroLineRequest request,
        CancellationToken cancellationToken)
    {
        await _metroLineService.RenameAsync(
            id,
            request.Name,
            cancellationToken);

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> Delete(
        int id,
        CancellationToken cancellationToken)
    {
        await _metroLineService.DeleteAsync(id, cancellationToken);

        return NoContent();
    }
}
