using MesoShqip.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MesoShqip.API.Controllers;

[ApiController]
[Route("api/v1/admin")]
[Authorize(Roles = "Admin")]
public class AdminController : ControllerBase
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken ct)
    {
        var users = await _context.Users
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Email,
                u.Role,
                u.NativeLanguage,
                u.Level,
                u.TotalPoints,
                u.CurrentStreak,
                u.OnboardingCompleted,
                u.CreatedAt
            })
            .ToListAsync(ct);
        return Ok(users);
    }

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats(CancellationToken ct)
    {
        var stats = new
        {
            TotalUsers = await _context.Users.CountAsync(u => u.Role != "Admin", ct),
            TotalLessons = await _context.Lessons.CountAsync(ct),
            TotalQuizzes = await _context.QuizSessions.CountAsync(ct),
            TotalStories = await _context.AiStories.CountAsync(ct),
            ActiveToday = await _context.Users.CountAsync(
                                   u => u.LastActivityDate >= DateTime.UtcNow.AddDays(-1), ct),
            CompletedLessons = await _context.LessonProgresses
                                   .CountAsync(p => p.Status == Domain.Enums.ProgressStatus.Completed, ct)
        };
        return Ok(stats);
    }

    [HttpGet("lessons")]
    public async Task<IActionResult> GetAllLessons(CancellationToken ct)
    {
        var lessons = await _context.Lessons
            .Select(l => new
            {
                l.Id,
                l.TitleAlbanian,
                l.TitleEnglish,
                l.Level,
                l.LessonType,
                l.IsPublished,
                l.OrderIndex,
                VocabCount = l.VocabularyItems.Count()
            })
            .OrderBy(l => l.OrderIndex)
            .ToListAsync(ct);
        return Ok(lessons);
    }

    [HttpPut("users/{userId}/role")]
    public async Task<IActionResult> ChangeUserRole(
        Guid userId, [FromBody] ChangeRoleRequest request, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync([userId], ct);
        if (user is null) return NotFound();
        user.Role = request.Role;
        await _context.SaveChangesAsync(ct);
        return Ok(new { message = "Roli u ndryshua me sukses." });
    }

    [HttpDelete("users/{userId}")]
    public async Task<IActionResult> DeleteUser(Guid userId, CancellationToken ct)
    {
        var user = await _context.Users.FindAsync([userId], ct);
        if (user is null) return NotFound();
        _context.Users.Remove(user);
        await _context.SaveChangesAsync(ct);
        return Ok(new { message = "Useri u fshi me sukses." });
    }

    [HttpPut("lessons/{lessonId}/publish")]
    public async Task<IActionResult> TogglePublish(Guid lessonId, CancellationToken ct)
    {
        var lesson = await _context.Lessons.FindAsync([lessonId], ct);
        if (lesson is null) return NotFound();
        lesson.IsPublished = !lesson.IsPublished;
        await _context.SaveChangesAsync(ct);
        return Ok(new { message = "Statusi u ndryshua.", isPublished = lesson.IsPublished });
    }
}

public record ChangeRoleRequest(string Role);