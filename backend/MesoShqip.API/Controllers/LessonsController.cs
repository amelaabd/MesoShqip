using MediatR;
using MesoShqip.Application.Features.Lessons.Queries;
using MesoShqip.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/lessons")]
[Authorize]
public class LessonsController : ControllerBase
{
    private readonly ISender _mediator;

    public LessonsController(ISender mediator)
    {
        _mediator = mediator;
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
}