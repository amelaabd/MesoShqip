using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Enums;
using MesoShqip.Domain.Interfaces;
using System.Text.Json;

namespace MesoShqip.Application.Features.AI.Commands;

public record GenerateStoryCommand(
    Guid ChildProfileId,
    IReadOnlyList<string> WordsToIntroduce
) : IRequest<Result<GenerateStoryResponse>>;

public record GenerateStoryResponse(
    Guid StoryId,
    string TitleAlbanian,
    string BodyAlbanian,
    string BodyTranslated,
    IReadOnlyList<NewWordEntry> NewWords
);

public class GenerateStoryHandler : IRequestHandler<GenerateStoryCommand, Result<GenerateStoryResponse>>
{
    private readonly IStoryGeneratorService _storyService;
    private readonly IRepository<ChildProfile> _childRepo;
    private readonly IRepository<AiStory> _storyRepo;
    private readonly IUnitOfWork _uow;

    public GenerateStoryHandler(
        IStoryGeneratorService storyService,
        IRepository<ChildProfile> childRepo,
        IRepository<AiStory> storyRepo,
        IUnitOfWork uow)
    {
        _storyService = storyService;
        _childRepo = childRepo;
        _storyRepo = storyRepo;
        _uow = uow;
    }

    public async Task<Result<GenerateStoryResponse>> Handle(
        GenerateStoryCommand request, CancellationToken ct)
    {
        var children = await _childRepo.FindAsync(c => c.Id == request.ChildProfileId, ct);
        var child = children.FirstOrDefault();
        if (child is null)
            return Result<GenerateStoryResponse>.Failure("Profili nuk u gjet.");

        var result = await _storyService.GenerateAsync(
            child.CurrentLevel,
            child.NativeLanguage,
            request.WordsToIntroduce,
            ct);

        var story = new AiStory
        {
            ChildProfileId = child.Id,
            TitleAlbanian = result.TitleAlbanian,
            BodyAlbanian = result.BodyAlbanian,
            BodyTranslated = result.BodyTranslated,
            VocabLevel = child.CurrentLevel,
            NewWordsJson = JsonSerializer.Serialize(result.NewWords)
        };

        await _storyRepo.AddAsync(story, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<GenerateStoryResponse>.Success(new GenerateStoryResponse(
            story.Id,
            story.TitleAlbanian,
            story.BodyAlbanian,
            story.BodyTranslated,
            result.NewWords));
    }
}