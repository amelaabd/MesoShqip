using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using MesoShqip.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MesoShqip.Infrastructure.Services.AI;

public class QuizGeneratorService : IQuizGeneratorService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _config;
    private readonly ILogger<QuizGeneratorService> _logger;

    public QuizGeneratorService(
        HttpClient httpClient,
        IConfiguration config,
        ILogger<QuizGeneratorService> logger)
    {
        _httpClient = httpClient;
        _config = config;
        _logger = logger;
    }

    public async Task<QuizGenerationResult> GenerateAsync(
        string nativeLanguage,
        IReadOnlyList<string> weakWords,
        CancellationToken ct = default)
    {
        var wordList = string.Join(", ", weakWords);

        var systemPrompt = """
            You are a language quiz creator for Albanian diaspora children.
            Create multiple-choice questions to test Albanian vocabulary.
            Always respond with valid JSON only — no markdown, no preamble.
            """;

        var userPrompt = "Create 5 multiple-choice quiz questions testing these Albanian words: " + wordList + ".\n" +
    "The child's native language is: " + nativeLanguage + ".\n" +
    "Mix question types: some ask for the translation, some ask for the Albanian word.\n\n" +
    "Respond ONLY with this exact JSON:\n" +
    "{\n" +
    "  \"questions\": [\n" +
    "    {\n" +
    "      \"questionText\": \"the question\",\n" +
    "      \"correctAnswer\": \"correct answer\",\n" +
    "      \"options\": [\"option1\", \"option2\", \"option3\", \"option4\"],\n" +
    "      \"explanation\": \"brief explanation in " + nativeLanguage + "\"\n" +
    "    }\n" +
    "  ]\n" +
    "}";

        var requestBody = new
        {
            model = _config["Anthropic:Model"] ?? "claude-opus-4-6",
            max_tokens = 1000,
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

        var quiz = JsonSerializer.Deserialize<QuizJson>(jsonText,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("Failed to parse quiz JSON.");

        return new QuizGenerationResult(
            quiz.Questions.Select(q =>
                new QuizQuestion(q.QuestionText, q.CorrectAnswer, q.Options, q.Explanation)).ToList());
    }

    private record AnthropicResponse(
        [property: JsonPropertyName("content")] List<ContentBlock> Content);

    private record ContentBlock(
        [property: JsonPropertyName("type")] string Type,
        [property: JsonPropertyName("text")] string Text);

    private record QuizJson(
        [property: JsonPropertyName("questions")] List<QuestionJson> Questions);

    private record QuestionJson(
        [property: JsonPropertyName("questionText")] string QuestionText,
        [property: JsonPropertyName("correctAnswer")] string CorrectAnswer,
        [property: JsonPropertyName("options")] List<string> Options,
        [property: JsonPropertyName("explanation")] string Explanation);
}