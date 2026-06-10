using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesoShqip.Domain.Entities;

namespace MesoShqip.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);
        builder.Property(u => u.Username).IsRequired().HasMaxLength(100);
        builder.Property(u => u.Email).IsRequired().HasMaxLength(256);
        builder.Property(u => u.PasswordHash).IsRequired().HasMaxLength(512);
        builder.Property(u => u.Role).IsRequired().HasMaxLength(50).HasDefaultValue("User");
        builder.Property(u => u.NativeLanguage).HasMaxLength(10).HasDefaultValue("en");
        builder.Property(u => u.TotalPoints).HasDefaultValue(0);
        builder.Property(u => u.CurrentStreak).HasDefaultValue(0);
        builder.Property(u => u.OnboardingCompleted).HasDefaultValue(false);
        builder.HasIndex(u => u.Email).IsUnique();
        builder.HasIndex(u => u.Username).IsUnique();

        builder.HasMany(u => u.LessonProgresses)
               .WithOne(lp => lp.User)
               .HasForeignKey(lp => lp.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.AiStories)
               .WithOne(s => s.User)
               .HasForeignKey(s => s.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.QuizSessions)
               .WithOne(q => q.User)
               .HasForeignKey(q => q.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.UserBadges)
               .WithOne(ub => ub.User)
               .HasForeignKey(ub => ub.UserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(u => u.PronunciationAttempts)
               .WithOne(p => p.User)
               .HasForeignKey(p => p.UserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}