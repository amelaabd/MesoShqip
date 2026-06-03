using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MesoShqip.Domain.Entities;

namespace MesoShqip.Infrastructure.Data.Configurations;

public class VocabularyItemConfiguration : IEntityTypeConfiguration<VocabularyItem>
{
    public void Configure(EntityTypeBuilder<VocabularyItem> builder)
    {
        builder.HasKey(v => v.Id);
        builder.Property(v => v.WordAlbanian).IsRequired().HasMaxLength(200);
        builder.Property(v => v.WordEnglish).IsRequired().HasMaxLength(200);
        builder.Property(v => v.ExampleSentence).HasMaxLength(500);
        builder.Property(v => v.AudioFileUrl).HasMaxLength(512);
        builder.Property(v => v.ImageUrl).HasMaxLength(512);
        builder.Property(v => v.Phonetic).HasMaxLength(100);
        builder.Property(v => v.DifficultyScore).HasDefaultValue(1);
    }
}