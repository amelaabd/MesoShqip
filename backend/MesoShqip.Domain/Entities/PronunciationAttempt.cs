using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class PronunciationAttempt : BaseEntity
{
    public Guid UserId { get; set; }
    public Guid VocabularyItemId { get; set; }
    public decimal AccuracyScore { get; set; }
    public string? SttTranscript { get; set; }

    public User User { get; set; } = null!;
    public VocabularyItem VocabularyItem { get; set; } = null!;
}