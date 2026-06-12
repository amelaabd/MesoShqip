using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Progress.Commands;

public record AwardBadgesCommand : IRequest<Result<List<string>>>;

public class AwardBadgesHandler : IRequestHandler<AwardBadgesCommand, Result<List<string>>>
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<UserBadge> _badgeRepo;
    private readonly IRepository<Badge> _badgeDefs;
    private readonly IRepository<LessonProgress> _progressRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public AwardBadgesHandler(
        IRepository<User> userRepo,
        IRepository<UserBadge> badgeRepo,
        IRepository<Badge> badgeDefs,
        IRepository<LessonProgress> progressRepo,
        IUnitOfWork uow,
        ICurrentUserService currentUser)
    {
        _userRepo = userRepo;
        _badgeRepo = badgeRepo;
        _badgeDefs = badgeDefs;
        _progressRepo = progressRepo;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<List<string>>> Handle(AwardBadgesCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;
        var users = await _userRepo.FindAsync(u => u.Id == userId, ct);
        var user = users.FirstOrDefault();
        if (user is null) return Result<List<string>>.Failure("User not found");

        var allBadges = await _badgeDefs.GetAllAsync(ct);
        var earnedBadges = await _badgeRepo.FindAsync(ub => ub.UserId == userId, ct);
        var earnedIds = earnedBadges.Select(eb => eb.BadgeId).ToHashSet();

        var progresses = await _progressRepo.FindAsync(p => p.UserId == userId, ct);
        var completed = progresses.Count(p => p.Status == Domain.Enums.ProgressStatus.Completed);

        var newlyEarned = new List<string>();

        foreach (var badge in allBadges.Where(b => !earnedIds.Contains(b.Id)))
        {
            bool earned = badge.Category switch
            {
                "Streak" => CheckStreak(badge.Name, user.CurrentStreak),
                "Points" => CheckPoints(badge.Name, user.TotalPoints),
                "Lessons" => CheckLessons(badge.Name, completed),
                "Level" => CheckLevel(badge.Name, user.Level.ToString()),
                _ => false
            };

            if (earned)
            {
                await _badgeRepo.AddAsync(new UserBadge
                {
                    UserId = userId,
                    BadgeId = badge.Id,
                    EarnedAt = DateTime.UtcNow
                }, ct);
                newlyEarned.Add(badge.Name);
            }
        }

        if (newlyEarned.Any())
            await _uow.SaveChangesAsync(ct);

        return Result<List<string>>.Success(newlyEarned);
    }

    private static bool CheckStreak(string name, int streak) => name switch
    {
        "3 Ditë" => streak >= 3,
        "7 Ditë" => streak >= 7,
        "30 Ditë" => streak >= 30,
        _ => false
    };

    private static bool CheckPoints(string name, int points) => name switch
    {
        "50 Pikë" => points >= 50,
        "200 Pikë" => points >= 200,
        "500 Pikë" => points >= 500,
        _ => false
    };

    private static bool CheckLessons(string name, int completed) => name switch
    {
        "Mësimi i Parë" => completed >= 1,
        "5 Mësime" => completed >= 5,
        "10 Mësime" => completed >= 10,
        _ => false
    };

    private static bool CheckLevel(string name, string level) => name switch
    {
        "Mesatar" => level is "Mesatar" or "Avancuar",
        "Avancuar" => level == "Avancuar",
        _ => false
    };
}