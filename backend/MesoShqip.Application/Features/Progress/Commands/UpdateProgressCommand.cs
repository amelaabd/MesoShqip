using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Progress.Commands;

public record UpdateProgressCommand(
    Guid ChildProfileId,
    Guid LessonId,
    int ScorePercent
) : IRequest<Result<bool>>;

public class UpdateProgressHandler : IRequestHandler<UpdateProgressCommand, Result<bool>>
{
    private readonly IRepository<LessonProgress> _progressRepo;
    private readonly IRepository<ChildProfile> _childRepo;
    private readonly IUnitOfWork _uow;

    public UpdateProgressHandler(
        IRepository<LessonProgress> progressRepo,
        IRepository<ChildProfile> childRepo,
        IUnitOfWork uow)
    {
        _progressRepo = progressRepo;
        _childRepo = childRepo;
        _uow = uow;
    }

    public async Task<Result<bool>> Handle(UpdateProgressCommand request, CancellationToken ct)
    {
        var progresses = await _progressRepo.FindAsync(
            p => p.ChildProfileId == request.ChildProfileId && p.LessonId == request.LessonId, ct);

        var progress = progresses.FirstOrDefault();

        if (progress is null)
        {
            progress = new LessonProgress
            {
                ChildProfileId = request.ChildProfileId,
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

        var children = await _childRepo.FindAsync(c => c.Id == request.ChildProfileId, ct);
        var child = children.FirstOrDefault();
        if (child is not null)
        {
            child.TotalPoints += request.ScorePercent >= 90 ? 20 : request.ScorePercent >= 60 ? 10 : 0;
            child.LastActivityDate = DateTime.UtcNow;
            _childRepo.Update(child);
        }

        await _uow.SaveChangesAsync(ct);
        return Result<bool>.Success(true);
    }
}