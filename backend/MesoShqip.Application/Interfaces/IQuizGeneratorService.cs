namespace MesoShqip.Application.Interfaces;

public interface IQuizGeneratorService
{
    Task<QuizGenerationResult> GenerateAsync(
        string nativeLanguage,
        IReadOnlyList<string> weakWords,
        CancellationToken ct = default);
}

public record QuizGenerationResult(
    IReadOnlyList<QuizQuestion> Questions
);

public record QuizQuestion(
    string QuestionText,
    string CorrectAnswer,
    IReadOnlyList<string> Options,
    string Explanation
);