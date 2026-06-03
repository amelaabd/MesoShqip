using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class PronunciationAttempt : BaseEntity
{
    public Guid ChildProfileId { get; set; }
    public Guid VocabularyItemId { get; set; }
    public decimal AccuracyScore { get; set; }
    public string? SttTranscript { get; set; }

    public ChildProfile ChildProfile { get; set; } = null!;
    public VocabularyItem VocabularyItem { get; set; } = null!;
}