using MesoShqip.Domain.Entities;

namespace MesoShqip.Application.Interfaces;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}