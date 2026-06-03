using MediatR;
using FluentValidation;
using MesoShqip.Application.Common.Models;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using MesoShqip.Domain.Interfaces;
using MesoShqip.Application.Interfaces;

namespace MesoShqip.Application.Features.Children.Commands;

public record CreateChildProfileCommand(
    string DisplayName,
    string AvatarCode,
    string NativeLanguage,
    LanguageLevel StartingLevel
) : IRequest<Result<ChildProfileResponse>>;

public record ChildProfileResponse(
    Guid Id,
    string DisplayName,
    string AvatarCode,
    string NativeLanguage,
    string CurrentLevel,
    int TotalPoints,
    int CurrentStreak
);

public class CreateChildProfileValidator : AbstractValidator<CreateChildProfileCommand>
{
    public CreateChildProfileValidator()
    {
        RuleFor(x => x.DisplayName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.NativeLanguage).NotEmpty().MaximumLength(10);
    }
}

public class CreateChildProfileHandler : IRequestHandler<CreateChildProfileCommand, Result<ChildProfileResponse>>
{
    private readonly IRepository<ChildProfile> _childRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CreateChildProfileHandler(IRepository<ChildProfile> childRepo, IUnitOfWork uow, ICurrentUserService currentUser)
    {
        _childRepo = childRepo;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<ChildProfileResponse>> Handle(CreateChildProfileCommand request, CancellationToken ct)
    {
        var child = new ChildProfile
        {
            ParentUserId = _currentUser.UserId,
            DisplayName = request.DisplayName,
            AvatarCode = request.AvatarCode,
            NativeLanguage = request.NativeLanguage,
            CurrentLevel = request.StartingLevel
        };

        await _childRepo.AddAsync(child, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<ChildProfileResponse>.Success(new ChildProfileResponse(
            child.Id, child.DisplayName, child.AvatarCode,
            child.NativeLanguage, child.CurrentLevel.ToString(),
            child.TotalPoints, child.CurrentStreak));
    }
}