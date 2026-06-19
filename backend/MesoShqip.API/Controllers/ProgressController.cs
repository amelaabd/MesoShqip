using MediatR;
using MesoShqip.Application.Features.Progress.Commands;
using MesoShqip.Application.Features.Progress.Queries;
using MesoShqip.Application.Interfaces;
using MesoShqip.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/progress")]
[Authorize]
public class ProgressController : ControllerBase
{
    private readonly ISender _mediator;
    private readonly AppDbContext _context;

    public ProgressController(ISender mediator, AppDbContext context)
    {
        _mediator = mediator;
        _context = context;
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

    [HttpPost("award-badges")]
    public async Task<IActionResult> AwardBadges(CancellationToken ct)
    {
        var result = await _mediator.Send(new AwardBadgesCommand(), ct);
        return Ok(result.Data);
    }

    [HttpGet("badges")]
    public async Task<IActionResult> GetBadges(CancellationToken ct)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var earned = await _context.UserBadges
            .Where(ub => ub.UserId == Guid.Parse(userId))
            .Select(ub => ub.BadgeId)
            .ToListAsync(ct);

        var allBadges = await _context.Badges
            .Select(b => new {
                b.Id,
                b.Name,
                b.Description,
                b.IconUrl,
                b.Category,
                IsEarned = earned.Contains(b.Id)
            })
            .ToListAsync(ct);

        return Ok(allBadges);
    }

    [HttpGet("leaderboard")]
    public async Task<IActionResult> GetLeaderboard(CancellationToken ct)
    {
        var leaderboard = await _context.Users
            .Where(u => u.Role != "Admin")
            .OrderByDescending(u => u.TotalPoints)
            .Take(20)
            .Select(u => new
            {
                u.Username,
                u.TotalPoints,
                u.CurrentStreak,
                Level = u.Level.ToString()
            })
            .ToListAsync(ct);

        return Ok(leaderboard);
    }

    [HttpGet("certificate")]
    public async Task<IActionResult> GetCertificate(
    [FromServices] ICertificateService certService, CancellationToken ct)
    {
        var userId = HttpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        if (userId is null) return Unauthorized();

        var user = await _context.Users.FindAsync([Guid.Parse(userId)], ct);
        if (user is null) return NotFound();

        var pdf = certService.GenerateCertificate(user.Username, user.Level.ToString(), DateTime.UtcNow);
        return File(pdf, "application/pdf", "certifikata.pdf");
    }
}