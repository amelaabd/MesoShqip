using MesoShqip.Domain.Common;
using MesoShqip.Domain.Enums;

namespace MesoShqip.Domain.Entities;

public class Lesson : BaseEntity
{
    public string TitleAlbanian { get; set; } = string.Empty;
    public string TitleEnglish { get; set; } = string.Empty;
    public LanguageLevel Level { get; set; }
    public LessonType LessonType { get; set; }
    public int OrderIndex { get; set; }
    public bool IsPublished { get; set; } = false;

    public ICollection<VocabularyItem> VocabularyItems { get; set; } = new List<VocabularyItem>();
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
}