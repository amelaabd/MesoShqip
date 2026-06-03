using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;
using MesoShqip.Application.Features.Children.Commands;

namespace MesoShqip.Application.Features.Children.Queries;

public record GetChildrenQuery : IRequest<Result<List<ChildProfileResponse>>>;

public class GetChildrenHandler : IRequestHandler<GetChildrenQuery, Result<List<ChildProfileResponse>>>
{
    private readonly IRepository<ChildProfile> _childRepo;
    private readonly ICurrentUserService _currentUser;

    public GetChildrenHandler(IRepository<ChildProfile> childRepo, ICurrentUserService currentUser)
    {
        _childRepo = childRepo;
        _currentUser = currentUser;
    }

    public async Task<Result<List<ChildProfileResponse>>> Handle(GetChildrenQuery request, CancellationToken ct)
    {
        var children = await _childRepo.FindAsync(c => c.ParentUserId == _currentUser.UserId, ct);

        var response = children.Select(c => new ChildProfileResponse(
            c.Id, c.DisplayName, c.AvatarCode,
            c.NativeLanguage, c.CurrentLevel.ToString(),
            c.TotalPoints, c.CurrentStreak)).ToList();

        return Result<List<ChildProfileResponse>>.Success(response);
    }
}