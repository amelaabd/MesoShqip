using MediatR;
using FluentValidation;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Auth.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;

public record LoginResponse(string AccessToken, string RefreshToken, string Username, string Role);

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly IRepository<User> _userRepo;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _uow;

    public LoginCommandHandler(IRepository<User> userRepo, ITokenService tokenService, IUnitOfWork uow)
    {
        _userRepo = userRepo;
        _tokenService = tokenService;
        _uow = uow;
    }

    public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken ct)
    {
        var users = await _userRepo.FindAsync(u => u.Email == request.Email, ct);
        var user = users.FirstOrDefault();

        if (user is null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Result<LoginResponse>.Failure("Email ose fjalëkalim i gabuar.");

        var accessToken = _tokenService.GenerateAccessToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiry = DateTime.UtcNow.AddDays(7);
        _userRepo.Update(user);
        await _uow.SaveChangesAsync(ct);

        return Result<LoginResponse>.Success(new LoginResponse(accessToken, refreshToken, user.Username, user.Role));
    }
}