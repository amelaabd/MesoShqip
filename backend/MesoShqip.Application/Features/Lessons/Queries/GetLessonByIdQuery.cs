using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Lessons.Queries;

public record GetLessonByIdQuery(Guid LessonId) : IRequest<Result<LessonDetailDto>>;

public record LessonDetailDto(
    Guid Id,
    string TitleAlbanian,
    string TitleEnglish,
    string Level,
    string LessonType,
    int OrderIndex,
    List<VocabularyDto> VocabularyItems
);

public record VocabularyDto(
    Guid Id,
    string WordAlbanian,
    string WordEnglish,
    string WordGerman,
    string WordItalian,
    string WordFrench,
    string WordSwedish,
    string WordTurkish,
    string? Phonetic,
    string? ExampleSentence,
    string? AudioFileUrl,
    string? ImageUrl,
    int DifficultyScore
);

public class GetLessonByIdHandler : IRequestHandler<GetLessonByIdQuery, Result<LessonDetailDto>>
{
    private readonly IRepository<Lesson> _lessonRepo;
    private readonly IRepository<VocabularyItem> _vocabRepo;

    public GetLessonByIdHandler(
        IRepository<Lesson> lessonRepo,
        IRepository<VocabularyItem> vocabRepo)
    {
        _lessonRepo = lessonRepo;
        _vocabRepo = vocabRepo;
    }

    public async Task<Result<LessonDetailDto>> Handle(
        GetLessonByIdQuery request, CancellationToken ct)
    {
        var lesson = await _lessonRepo.GetByIdAsync(request.LessonId, ct);
        if (lesson is null)
            return Result<LessonDetailDto>.Failure("Mësimi nuk u gjet.");

        var vocabItems = await _vocabRepo.FindAsync(v => v.LessonId == lesson.Id, ct);

        var vocabDtos = vocabItems.Select(v => new VocabularyDto(
            v.Id,
            v.WordAlbanian,
            v.WordEnglish,
            v.WordGerman,
            v.WordItalian,
            v.WordFrench,
            v.WordSwedish,
            v.WordTurkish,
            v.Phonetic,
            v.ExampleSentence,
            v.AudioFileUrl,
            v.ImageUrl,
            v.DifficultyScore)).ToList();

        return Result<LessonDetailDto>.Success(new LessonDetailDto(
            lesson.Id,
            lesson.TitleAlbanian,
            lesson.TitleEnglish,
            lesson.Level.ToString(),
            lesson.LessonType.ToString(),
            lesson.OrderIndex,
            vocabDtos));
    }
}