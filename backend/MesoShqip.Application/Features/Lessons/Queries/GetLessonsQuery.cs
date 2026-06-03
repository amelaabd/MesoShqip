using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using MesoShqip.Domain.Interfaces;

namespace MesoShqip.Application.Features.Lessons.Queries;

public record GetLessonsQuery(LanguageLevel? Level = null) : IRequest<Result<List<LessonDto>>>;

public record LessonDto(
    Guid Id,
    string TitleAlbanian,
    string TitleEnglish,
    string Level,
    string LessonType,
    int OrderIndex,
    int VocabularyCount
);

public class GetLessonsHandler : IRequestHandler<GetLessonsQuery, Result<List<LessonDto>>>
{
    private readonly IRepository<Lesson> _lessonRepo;
    private readonly IRepository<VocabularyItem> _vocabRepo;

    public GetLessonsHandler(
        IRepository<Lesson> lessonRepo,
        IRepository<VocabularyItem> vocabRepo)
    {
        _lessonRepo = lessonRepo;
        _vocabRepo = vocabRepo;
    }

    public async Task<Result<List<LessonDto>>> Handle(GetLessonsQuery request, CancellationToken ct)
    {
        var lessons = request.Level.HasValue
            ? await _lessonRepo.FindAsync(l => l.IsPublished && l.Level == request.Level.Value, ct)
            : await _lessonRepo.FindAsync(l => l.IsPublished, ct);

        var dtos = new List<LessonDto>();
        foreach (var lesson in lessons.OrderBy(l => l.OrderIndex))
        {
            var vocab = await _vocabRepo.FindAsync(v => v.LessonId == lesson.Id, ct);
            dtos.Add(new LessonDto(
                lesson.Id,
                lesson.TitleAlbanian,
                lesson.TitleEnglish,
                lesson.Level.ToString(),
                lesson.LessonType.ToString(),
                lesson.OrderIndex,
                vocab.Count));
        }

        return Result<List<LessonDto>>.Success(dtos);
    }
}