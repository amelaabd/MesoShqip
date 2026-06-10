using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Auth.Commands;

public record CompleteOnboardingCommand(
    string NativeLanguage,
    LanguageLevel Level
) : IRequest<Result<bool>>;

public class CompleteOnboardingHandler : IRequestHandler<CompleteOnboardingCommand, Result<bool>>
{
    private readonly IRepository<User> _userRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public CompleteOnboardingHandler(
        IRepository<User> userRepo,
        IUnitOfWork uow,
        ICurrentUserService currentUser)
    {
        _userRepo = userRepo;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<bool>> Handle(CompleteOnboardingCommand request, CancellationToken ct)
    {
        var users = await _userRepo.FindAsync(u => u.Id == _currentUser.UserId, ct);
        var user = users.FirstOrDefault();
        if (user is null)
            return Result<bool>.Failure("Useri nuk u gjet.");

        user.NativeLanguage = request.NativeLanguage;
        user.Level = request.Level;
        user.OnboardingCompleted = true;
        _userRepo.Update(user);
        await _uow.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}