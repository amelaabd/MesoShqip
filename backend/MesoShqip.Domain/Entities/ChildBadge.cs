using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class ChildBadge : BaseEntity
{
    public Guid ChildProfileId { get; set; }
    public Guid BadgeId { get; set; }
    public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

    public ChildProfile ChildProfile { get; set; } = null!;
    public Badge Badge { get; set; } = null!;
}