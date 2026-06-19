using MediatR;
using MesoShqip.Application.Features.Lessons.Queries;
using MesoShqip.Domain.Enums;
using MesoShqip.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/lessons")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly AppDbContext _context;

    public LessonsController(ISender mediator, AppDbContext context)
    {
        _mediator = mediator;
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetLessons(
        [FromQuery] LanguageLevel? level, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLessonsQuery(level), ct);
        return Ok(result.Data);
    }

    [HttpGet("{lessonId}")]
    public async Task<IActionResult> GetLesson(Guid lessonId, CancellationToken ct)
    {
        var result = await _mediator.Send(new GetLessonByIdQuery(lessonId), ct);
        if (!result.IsSuccess)
            return NotFound(new { error = result.Error });
        return Ok(result.Data);
    }

    [HttpGet("with-progress")]
    public async Task<IActionResult> GetLessonsWithProgress(CancellationToken ct)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var lessons = await _context.Lessons
            .Where(l => l.IsPublished)
            .OrderBy(l => l.OrderIndex)
            .Select(l => new
            {
                l.Id,
                l.TitleAlbanian,
                l.TitleEnglish,
                Level = l.Level.ToString(),
                LessonType = l.LessonType.ToString(),
                l.OrderIndex,
                VocabularyCount = l.VocabularyItems.Count(),
                Progress = _context.LessonProgresses
                    .Where(p => p.LessonId == l.Id && p.UserId == Guid.Parse(userId))
                    .Select(p => p.ScorePercent)
                    .FirstOrDefault()
            })
            .ToListAsync(ct);

        return Ok(lessons);
    }
}