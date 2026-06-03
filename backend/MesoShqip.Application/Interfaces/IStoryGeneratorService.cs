using MesoShqip.Domain.Enums;

namespace MesoShqip.Application.Interfaces;

public interface IStoryGeneratorService
{
    Task<StoryGenerationResult> GenerateAsync(
        LanguageLevel level,
        string nativeLanguage,
        IReadOnlyList<string> wordsToIntroduce,
        CancellationToken ct = default);
}

public record StoryGenerationResult(
    string TitleAlbanian,
    string BodyAlbanian,
    string BodyTranslated,
    IReadOnlyList<NewWordEntry> NewWords
);

public record NewWordEntry(
    string Albanian,
    string Translated,
    string Phonetic
);