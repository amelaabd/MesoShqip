using MediatR;
using MesoShqip.Application.Features.Children.Commands;
using MesoShqip.Application.Features.Children.Queries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/children")]
[Authorize]
public class ChildrenController : ControllerBase
{
    private readonly ISender _mediator;

    public ChildrenController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetChildren(CancellationToken ct)
    {
        var result = await _mediator.Send(new GetChildrenQuery(), ct);
        return Ok(result.Data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateChild(
        [FromBody] CreateChildProfileCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return CreatedAtAction(nameof(GetChildren), result.Data);
    }
}