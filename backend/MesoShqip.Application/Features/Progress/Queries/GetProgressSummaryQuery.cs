using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Progress.Queries;

public record GetProgressSummaryQuery(Guid ChildProfileId) : IRequest<Result<ProgressSummaryResponse>>;

public record ProgressSummaryResponse(
    string DisplayName,
    string CurrentLevel,
    int TotalPoints,
    int CurrentStreak,
    int CompletedLessons,
    int TotalLessons
);

public class GetProgressSummaryHandler : IRequestHandler<GetProgressSummaryQuery, Result<ProgressSummaryResponse>>
{
    private readonly IRepository<ChildProfile> _childRepo;
    private readonly IRepository<LessonProgress> _progressRepo;

    public GetProgressSummaryHandler(IRepository<ChildProfile> childRepo, IRepository<LessonProgress> progressRepo)
    {
        _childRepo = childRepo;
        _progressRepo = progressRepo;
    }

    public async Task<Result<ProgressSummaryResponse>> Handle(GetProgressSummaryQuery request, CancellationToken ct)
    {
        var children = await _childRepo.FindAsync(c => c.Id == request.ChildProfileId, ct);
        var child = children.FirstOrDefault();
        if (child is null)
            return Result<ProgressSummaryResponse>.Failure("Profili nuk u gjet.");

        var progresses = await _progressRepo.FindAsync(p => p.ChildProfileId == request.ChildProfileId, ct);
        var completed = progresses.Count(p => p.Status == Domain.Enums.ProgressStatus.Completed);

        return Result<ProgressSummaryResponse>.Success(new ProgressSummaryResponse(
            child.DisplayName,
            child.CurrentLevel.ToString(),
            child.TotalPoints,
            child.CurrentStreak,
            completed,
            progresses.Count));
    }
}