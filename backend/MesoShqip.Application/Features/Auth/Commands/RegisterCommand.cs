using MediatR;
using FluentValidation;
using MesoShqip.Application.Common.Models;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Auth.Commands;

public record RegisterCommand(
    string Username,
    string Email,
    string Password
) : IRequest<Result<RegisterResponse>>;

public record RegisterResponse(Guid UserId, string Username, string Email);

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Username).NotEmpty().MinimumLength(3).MaximumLength(100);
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(6);
    }
}

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterResponse>>
{
    private readonly IRepository<User> _userRepo;
    private readonly IUnitOfWork _uow;

    public RegisterCommandHandler(IRepository<User> userRepo, IUnitOfWork uow)
    {
        _userRepo = userRepo;
        _uow = uow;
    }

    public async Task<Result<RegisterResponse>> Handle(RegisterCommand request, CancellationToken ct)
    {
        var existing = await _userRepo.FindAsync(u => u.Email == request.Email, ct);
        if (existing.Any())
            return Result<RegisterResponse>.Failure("Email është tashmë i regjistruar.");

        var user = new User
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        await _userRepo.AddAsync(user, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<RegisterResponse>.Success(new RegisterResponse(user.Id, user.Username, user.Email));
    }
}