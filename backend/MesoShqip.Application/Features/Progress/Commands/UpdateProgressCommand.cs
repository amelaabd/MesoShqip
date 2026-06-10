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
) : IRequest<Result<bool>>;

public class UpdateProgressHandler : IRequestHandler<UpdateProgressCommand, Result<bool>>
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

    public async Task<Result<bool>> Handle(UpdateProgressCommand request, CancellationToken ct)
    {
        var userId = _currentUser.UserId;

        var progresses = await _progressRepo.FindAsync(
            p => p.UserId == userId && p.LessonId == request.LessonId, ct);

        var progress = progresses.FirstOrDefault();

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
        }
        else
        {
            progress.ScorePercent = Math.Max(progress.ScorePercent, request.ScorePercent);
            progress.AttemptsCount++;
            if (request.ScorePercent >= 60)
            {
                progress.Status = ProgressStatus.Completed;
                progress.CompletedAt ??= DateTime.UtcNow;
            }
            _progressRepo.Update(progress);
        }

        var users = await _userRepo.FindAsync(u => u.Id == userId, ct);
        var user = users.FirstOrDefault();
        if (user is not null)
        {
            user.TotalPoints += request.ScorePercent >= 90 ? 20 : request.ScorePercent >= 60 ? 10 : 0;
            user.LastActivityDate = DateTime.UtcNow;
            _userRepo.Update(user);
        }

        await _uow.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}