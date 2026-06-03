using MediatR;
using MesoShqip.Application.Features.AI.Commands;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/ai")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly ISender _mediator;

    public AiController(ISender mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("stories/generate")]
    public async Task<IActionResult> GenerateStory(
        [FromBody] GenerateStoryCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return CreatedAtAction(nameof(GenerateStory), new { id = result.Data!.StoryId }, result.Data);
    }

    [HttpPost("quizzes/generate")]
    public async Task<IActionResult> GenerateQuiz(
        [FromBody] GenerateQuizCommand command, CancellationToken ct)
    {
        var result = await _mediator.Send(command, ct);
        if (!result.IsSuccess)
            return BadRequest(new { error = result.Error });
        return Ok(result.Data);
    }
}