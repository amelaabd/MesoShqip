using MediatR;
using MesoShqip.Application.Features.AI.Commands;
using MesoShqip.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/ai")]
[Authorize]
public class AiController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly AppDbContext _context;

    public AiController(ISender mediator, AppDbContext context)
    {
        _mediator = mediator;
        _context = context;
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

    [HttpGet("stories/{childId}")]
    public async Task<IActionResult> GetStories(Guid childId, CancellationToken ct)
    {
        var stories = await _context.AiStories
            .Where(s => s.ChildProfileId == childId)
            .OrderByDescending(s => s.CreatedAt)
            .Select(s => new
            {
                s.Id,
                s.TitleAlbanian,
                s.BodyAlbanian,
                s.BodyTranslated,
                s.VocabLevel,
                s.NewWordsJson,
                s.IsRead,
                GeneratedAt = s.CreatedAt
            })
            .ToListAsync(ct);
        return Ok(stories);
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