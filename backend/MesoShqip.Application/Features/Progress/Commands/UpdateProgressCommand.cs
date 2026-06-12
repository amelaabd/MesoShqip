using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Progress.Commands;

public record UpdateProgressCommand(
    Guid LessonId,
    int ScorePercent
) : IRequest<Result<UpdateProgressResponse>>;

public record UpdateProgressResponse(
    bool IsSuccess,
    int PointsEarned,
    int NewStreak,
    int TotalPoints,
    bool LeveledUp,
    string NewLevel
);

public class UpdateProgressHandler : IRequestHandler<UpdateProgressCommand, Result<UpdateProgressResponse>>
{
    private readonly IRepository<LessonProgress> _progressRepo;
    private readonly IRepository<User> _userRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public UpdateProgressHandler(
        IRepository<LessonProgress> progressRepo,
        IRepository<User> userRepo,
        IUnitOfWork uow,
        ICurrentUserService currentUser)
    {
        _progressRepo = progressRepo;
        _userRepo = userRepo;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<UpdateProgressResponse>> Handle(
        UpdateProgressCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        var progresses = await _progressRepo.FindAsync(
            p => p.UserId == userId && p.LessonId == request.LessonId, ct);

        var progress = progresses.FirstOrDefault();
        int pointsEarned = 0;

        if (progress is null)
        {
            progress = new LessonProgress
            {
                UserId = userId,
                LessonId = request.LessonId,
                ScorePercent = request.ScorePercent,
                AttemptsCount = 1,
                Status = request.ScorePercent >= 60 ? ProgressStatus.Completed : ProgressStatus.InProgress,
                CompletedAt = request.ScorePercent >= 60 ? DateTime.UtcNow : null
            };
            await _progressRepo.AddAsync(progress, ct);
            pointsEarned = request.ScorePercent >= 90 ? 20 : request.ScorePercent >= 60 ? 10 : 3;
        }
        else
        {
            bool wasCompleted = progress.Status == ProgressStatus.Completed;
            progress.ScorePercent = Math.Max(progress.ScorePercent, request.ScorePercent);
            progress.AttemptsCount++;

            if (request.ScorePercent >= 60)
            {
                progress.Status = ProgressStatus.Completed;
                progress.CompletedAt ??= DateTime.UtcNow;
            }
            _progressRepo.Update(progress);
            pointsEarned = !wasCompleted && request.ScorePercent >= 60 ? 10 : 2;
        }

        var users = await _userRepo.FindAsync(u => u.Id == userId, ct);
        var user = users.FirstOrDefault();

        string oldLevel = user?.Level.ToString() ?? "Fillestor";
        bool leveledUp = false;

        if (user is not null)
        {
            user.TotalPoints += pointsEarned;
            UpdateStreak(user);
            leveledUp = UpdateLevel(user);
            user.LastActivityDate = DateTime.UtcNow;
            _userRepo.Update(user);
        }

        await _uow.SaveChangesAsync(ct);

        return Result<UpdateProgressResponse>.Success(new UpdateProgressResponse(
            true,
            pointsEarned,
            user?.CurrentStreak ?? 0,
            user?.TotalPoints ?? 0,
            leveledUp,
            user?.Level.ToString() ?? oldLevel));
    }

    private static void UpdateStreak(User user)
    {
        var today = DateTime.UtcNow.Date;
        var lastActivity = user.LastActivityDate?.Date;

        if (lastActivity == null)
        {
            user.CurrentStreak = 1;
        }
        else if (lastActivity == today)
        {
            // Already active today — no change
        }
        else if (lastActivity == today.AddDays(-1))
        {
            // Consecutive day
            user.CurrentStreak++;
        }
        else
        {
            // Streak broken
            user.CurrentStreak = 1;
        }
    }

    private static bool UpdateLevel(User user)
    {
        var oldLevel = user.Level;
        user.Level = user.TotalPoints switch
        {
            >= 500 => LanguageLevel.Avancuar,
            >= 150 => LanguageLevel.Mesatar,
            _ => LanguageLevel.Fillestor,
        };
        return user.Level != oldLevel;
    }
}