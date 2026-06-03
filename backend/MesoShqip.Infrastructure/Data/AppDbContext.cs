using Microsoft.EntityFrameworkCore;
using MesoShqip.Domain.Entities;
using System.Reflection;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace MesoShqip.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<ChildProfile> ChildProfiles => Set<ChildProfile>();
    public DbSet<Lesson> Lessons => Set<Lesson>();
    public DbSet<VocabularyItem> VocabularyItems => Set<VocabularyItem>();
    public DbSet<LessonProgress> LessonProgresses => Set<LessonProgress>();
    public DbSet<AiStory> AiStories => Set<AiStory>();
    public DbSet<QuizSession> QuizSessions => Set<QuizSession>();
    public DbSet<PronunciationAttempt> PronunciationAttempts => Set<PronunciationAttempt>();
    public DbSet<Badge> Badges => Set<Badge>();
    public DbSet<ChildBadge> ChildBadges => Set<ChildBadge>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        foreach (var entry in ChangeTracker.Entries<MesoShqip.Domain.Common.BaseEntity>())
        {
            if (entry.State == EntityState.Modified)
                entry.Entity.UpdatedAt = DateTime.UtcNow;
        }
        return base.SaveChangesAsync(ct);
    }
}