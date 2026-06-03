using MesoShqip.Domain.Common;
using MesoShqip.Domain.Enums;

namespace MesoShqip.Domain.Entities;

public class LessonProgress : BaseEntity
{
    public Guid ChildProfileId { get; set; }
    public Guid LessonId { get; set; }
    public ProgressStatus Status { get; set; } = ProgressStatus.NotStarted;
    public int ScorePercent { get; set; } = 0;
    public int AttemptsCount { get; set; } = 0;
    public DateTime? CompletedAt { get; set; }

    public ChildProfile ChildProfile { get; set; } = null!;
    public Lesson Lesson { get; set; } = null!;
}