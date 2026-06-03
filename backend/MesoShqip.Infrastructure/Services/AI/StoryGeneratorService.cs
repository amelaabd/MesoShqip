using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MesoShqip.Infrastructure.Services.AI;

public class StoryGeneratorService : IStoryGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<StoryGeneratorService> _logger;

    public StoryGeneratorService(
        HttpClient httpClient,
        IConfiguration config,
        ILogger<StoryGeneratorService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<StoryGenerationResult> GenerateAsync(
        LanguageLevel level,
        string nativeLanguage,
        IReadOnlyList<string> wordsToIntroduce,
        CancellationToken ct = default)
    {
        var levelDesc = level switch
        {
            LanguageLevel.Fillestor => "beginner (ages 5-7, very simple sentences, max 100 words)",
            LanguageLevel.Mesatar => "intermediate (ages 8-10, short paragraphs, max 150 words)",
            LanguageLevel.Avancuar => "advanced (ages 11-13, richer narrative, max 200 words)",
            _ => "beginner"
        };

        var wordList = string.Join(", ", wordsToIntroduce);

        var systemPrompt = """
            You are a children's story writer for Albanian diaspora families.
            Write warm, imaginative stories celebrating Albanian culture and nature.
            Always respond with valid JSON only — no markdown, no preamble, no explanation.
            """;

        var userPrompt = "Write a child-friendly Albanian story at " + levelDesc + " level.\n" +
            "Naturally introduce these Albanian words: " + wordList + ".\n" +
            "The translation language is: " + nativeLanguage + ".\n\n" +
            "Respond ONLY with this exact JSON structure:\n" +
            "{\n" +
            "  \"titleAlbanian\": \"story title in Albanian\",\n" +
            "  \"bodyAlbanian\": \"full story in Albanian\",\n" +
            "  \"bodyTranslated\": \"full translation in " + nativeLanguage + "\",\n" +
            "  \"newWords\": [\n" +
            "    {\n" +
            "      \"albanian\": \"fjala\",\n" +
            "      \"translated\": \"the word in " + nativeLanguage + "\",\n" +
            "      \"phonetic\": \"FYAH-la\"\n" +
            "    }\n" +
            "  ]\n" +
            "}";

        var requestBody = new
        {
            model = _config["Anthropic:Model"] ?? "claude-opus-4-6",
            max_tokens = int.Parse(_config["Anthropic:MaxTokens"] ?? "1500"),
            system = systemPrompt,
            messages = new[]
            {
                new { role = "user", content = userPrompt }
            }
        };

        var response = await _httpClient.PostAsJsonAsync(
            "https://api.anthropic.com/v1/messages", requestBody, ct);

        response.EnsureSuccessStatusCode();

        var apiResponse = await response.Content
            .ReadFromJsonAsync<AnthropicResponse>(cancellationToken: ct)
            ?? throw new InvalidOperationException("Empty response from Anthropic.");

        var jsonText = apiResponse.Content
            .First(c => c.Type == "text").Text;

        var story = JsonSerializer.Deserialize<StoryJson>(jsonText,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Failed to parse story JSON.");

        return new StoryGenerationResult(
            story.TitleAlbanian,
            story.BodyAlbanian,
            story.BodyTranslated,
            story.NewWords.Select(w =>
                new NewWordEntry(w.Albanian, w.Translated, w.Phonetic)).ToList());
    }

    private record AnthropicResponse(
        [property: JsonPropertyName("content")] List<ContentBlock> Content);

    private record ContentBlock(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("text")] string Text);

    private record StoryJson(
        [property: JsonPropertyName("titleAlbanian")] string TitleAlbanian,
        [property: JsonPropertyName("bodyAlbanian")] string BodyAlbanian,
        [property: JsonPropertyName("bodyTranslated")] string BodyTranslated,
        [property: JsonPropertyName("newWords")] List<WordJson> NewWords);

    private record WordJson(
        [property: JsonPropertyName("albanian")] string Albanian,
        [property: JsonPropertyName("translated")] string Translated,
        [property: JsonPropertyName("phonetic")] string Phonetic);
}