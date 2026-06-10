using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesoShqip.Domain.Entities;

namespace MesoShqip.Infrastructure.Data.Configurations;

public class LessonProgressConfiguration : IEntityTypeConfiguration<LessonProgress>
{
    public void Configure(EntityTypeBuilder<LessonProgress> builder)
    {
        builder.HasKey(lp => lp.Id);
        builder.HasIndex(lp => new { lp.UserId, lp.LessonId }).IsUnique();
        builder.Property(lp => lp.ScorePercent).HasDefaultValue(0);
        builder.Property(lp => lp.AttemptsCount).HasDefaultValue(0);
    }
}