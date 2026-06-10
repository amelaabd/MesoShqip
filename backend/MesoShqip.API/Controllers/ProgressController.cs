using MediatR;
using MesoShqip.Application.Features.Progress.Commands;
using MesoShqip.Application.Features.Progress.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/progress")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly ISender _mediator;

    public ProgressController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("summary")]
    public async Task<IActionResult> GetSummary(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetProgressSummaryQuery(), ct);
        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpPost("update")]
    public async Task<IActionResult> UpdateProgress(
        [FromBody] UpdateProgressCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(new { message = "Progresi u përditësua." });
    }
}