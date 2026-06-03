using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "Parent";
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    public ICollection<ChildProfile> ChildProfiles { get; set; } = new List<ChildProfile>();
}