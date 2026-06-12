using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class VocabularyItem : BaseEntity
{
    public Guid LessonId { get; set; }
    public string WordAlbanian { get; set; } = string.Empty;
    public string WordEnglish { get; set; } = string.Empty;
    public string WordGerman { get; set; } = string.Empty;
    public string WordItalian { get; set; } = string.Empty;
    public string WordFrench { get; set; } = string.Empty;
    public string WordSwedish { get; set; } = string.Empty;
    public string WordTurkish { get; set; } = string.Empty;
    public string? ExampleSentence { get; set; }
    public string? AudioFileUrl { get; set; }
    public string? ImageUrl { get; set; }
    public string? PartOfSpeech { get; set; }
    public string? Phonetic { get; set; }
    public int DifficultyScore { get; set; } = 1;

    public Lesson Lesson { get; set; } = null!;
    public ICollection<PronunciationAttempt> PronunciationAttempts { get; set; } = new List<PronunciationAttempt>();
}