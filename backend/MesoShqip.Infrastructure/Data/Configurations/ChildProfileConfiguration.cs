using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesoShqip.Domain.Entities;

namespace MesoShqip.Infrastructure.Data.Configurations;

public class ChildProfileConfiguration : IEntityTypeConfiguration<ChildProfile>
{
    public void Configure(EntityTypeBuilder<ChildProfile> builder)
    {
        builder.HasKey(c => c.Id);
        builder.Property(c => c.DisplayName).IsRequired().HasMaxLength(100);
        builder.Property(c => c.AvatarCode).HasMaxLength(20).HasDefaultValue("eagle");
        builder.Property(c => c.NativeLanguage).HasMaxLength(10).HasDefaultValue("en");
        builder.Property(c => c.TotalPoints).HasDefaultValue(0);
        builder.Property(c => c.CurrentStreak).HasDefaultValue(0);

        builder.HasMany(c => c.LessonProgresses)
               .WithOne(lp => lp.ChildProfile)
               .HasForeignKey(lp => lp.ChildProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.AiStories)
               .WithOne(s => s.ChildProfile)
               .HasForeignKey(s => s.ChildProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.QuizSessions)
               .WithOne(q => q.ChildProfile)
               .HasForeignKey(q => q.ChildProfileId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(c => c.ChildBadges)
               .WithOne(cb => cb.ChildProfile)
               .HasForeignKey(cb => cb.ChildProfileId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}