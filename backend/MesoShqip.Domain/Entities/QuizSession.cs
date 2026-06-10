using MesoShqip.Domain.Common;

namespace MesoShqip.Domain.Entities;

public class QuizSession : BaseEntity
{
    public Guid UserId { get; set; }
    public string QuizType { get; set; } = "Vocabulary";
    public string QuestionsJson { get; set; } = "[]";
    public int TotalQuestions { get; set; }
    public int CorrectAnswers { get; set; } = 0;
    public int PointsEarned { get; set; } = 0;
    public DateTime? CompletedAt { get; set; }

    public User User { get; set; } = null!;
}