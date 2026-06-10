using MesoShqip.Domain.Common;
using MesoShqip.Domain.Enums;

namespace MesoShqip.Domain.Entities;

public class User : BaseEntity
{
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string Role { get; set; } = "User";
    public string? RefreshToken { get; set; }
    public DateTime? RefreshTokenExpiry { get; set; }

    // Profili i mësimit
    public string NativeLanguage { get; set; } = "en";
    public LanguageLevel Level { get; set; } = LanguageLevel.Fillestor;
    public int TotalPoints { get; set; } = 0;
    public int CurrentStreak { get; set; } = 0;
    public DateTime? LastActivityDate { get; set; }
    public bool OnboardingCompleted { get; set; } = false;

    public ICollection<LessonProgress> LessonProgresses { get; set; } = new List<LessonProgress>();
    public ICollection<AiStory> AiStories { get; set; } = new List<AiStory>();
    public ICollection<QuizSession> QuizSessions { get; set; } = new List<QuizSession>();
    public ICollection<UserBadge> UserBadges { get; set; } = new List<UserBadge>();
    public ICollection<PronunciationAttempt> PronunciationAttempts { get; set; } = new List<PronunciationAttempt>();
}