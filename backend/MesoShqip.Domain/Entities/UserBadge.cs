using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class UserBadge : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid BadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}