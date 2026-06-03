using MesoShqip.Domain.Common;
using MesoShqip.Domain.Enums;

namespace MesoShqip.Domain.Entities;

public class ChildProfile : BaseEntity
{
    public Guid ParentUserId { get; set; }
    public string DisplayName { get; set; } = string.Empty;
    public string AvatarCode { get; set; } = "eagle";
    public LanguageLevel CurrentLevel { get; set; } = LanguageLevel.Fillestor;
    public string NativeLanguage { get; set; } = "en";
    public int TotalPoints { get; set; } = 0;
    public int CurrentStreak { get; set; } = 0;
    public DateTime? LastActivityDate { get; set; }

    public User Parent { get; set; } = null!;
    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public ICollection<AiStory> AiStories { get; set; } = new List<AiStory>();
    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
    public ICollection<ChildBadge> ChildBadges { get; set; } = new List<ChildBadge>();
}