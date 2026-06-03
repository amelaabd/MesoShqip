using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesoShqip.Domain.Entities;

namespace MesoShqip.Infrastructure.Data.Configurations;

public class LessonConfiguration : IEntityTypeConfiguration<Lesson>
{
    public void Configure(EntityTypeBuilder<Lesson> builder)
    {
        builder.HasKey(l => l.Id);
        builder.Property(l => l.TitleAlbanian).IsRequired().HasMaxLength(200);
        builder.Property(l => l.TitleEnglish).IsRequired().HasMaxLength(200);
        builder.Property(l => l.IsPublished).HasDefaultValue(false);

        builder.HasMany(l => l.VocabularyItems)
               .WithOne(v => v.Lesson)
               .HasForeignKey(v => v.LessonId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}