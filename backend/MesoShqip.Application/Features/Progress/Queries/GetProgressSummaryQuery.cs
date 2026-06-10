using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Progress.Queries;

public record GetProgressSummaryQuery : IRequest<Result<ProgressSummaryResponse>>;

public record ProgressSummaryResponse(
    string Username,
    string NativeLanguage,
    string CurrentLevel,
    int TotalPoints,
    int CurrentStreak,
    int CompletedLessons,
    int TotalLessons
);

public class GetProgressSummaryHandler : IRequestHandler<GetProgressSummaryQuery, Result<ProgressSummaryResponse>>
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<LessonProgress> _progressRepo;
    private readonly ICurrentUserService _currentUser;

    public GetProgressSummaryHandler(
        IRepository<User> userRepo,
        IRepository<LessonProgress> progressRepo,
        ICurrentUserService currentUser)
    {
        _userRepo = userRepo;
        _progressRepo = progressRepo;
        _currentUser = currentUser;
    }

    public async Task<Result<ProgressSummaryResponse>> Handle(
        GetProgressSummaryQuery request, CancellationToken ct)
    {
        var users = await _userRepo.FindAsync(u => u.Id == _currentUser.UserId, ct);
        var user = users.FirstOrDefault();
        if (user is null)
            return Result<ProgressSummaryResponse>.Failure("Useri nuk u gjet.");

        var progresses = await _progressRepo.FindAsync(p => p.UserId == user.Id, ct);
        var completed = progresses.Count(p => p.Status == Domain.Enums.ProgressStatus.Completed);

        return Result<ProgressSummaryResponse>.Success(new ProgressSummaryResponse(
            user.Username,
            user.NativeLanguage,
            user.Level.ToString(),
            user.TotalPoints,
            user.CurrentStreak,
            completed,
            progresses.Count));
    }
}