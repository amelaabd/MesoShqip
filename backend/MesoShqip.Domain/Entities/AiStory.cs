using MesoShqip.Domain.Common;
using MesoShqip.Domain.Enums;

namespace MesoShqip.Domain.Entities;

public class AiStory : BaseEntity
{
    public Guid UserId { get; set; }
    public string TitleAlbanian { get; set; } = string.Empty;
    public string BodyAlbanian { get; set; } = string.Empty;
    public string BodyTranslated { get; set; } = string.Empty;
    public LanguageLevel VocabLevel { get; set; }
    public string NewWordsJson { get; set; } = "[]";
    public bool IsRead { get; set; } = false;

    public User User { get; set; } = null!;
}