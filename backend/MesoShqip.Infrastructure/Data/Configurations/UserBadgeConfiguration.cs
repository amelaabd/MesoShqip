using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesoShqip.Domain.Entities;

namespace MesoShqip.Infrastructure.Data.Configurations;

public class UserBadgeConfiguration : IEntityTypeConfiguration<UserBadge>
{
    public void Configure(EntityTypeBuilder<UserBadge> builder)
    {
        builder.HasKey(ub => ub.Id);
        builder.HasIndex(ub => new { ub.UserId, ub.BadgeId }).IsUnique();
    }
}