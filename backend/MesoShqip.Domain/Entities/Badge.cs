using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class Badge : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string IconUrl { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string CriteriaJson { get; set; } = "{}";

    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
}