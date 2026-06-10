using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;
using System.Text.Json;

namespace MesoShqip.Application.Features.AI.Commands;

public record GenerateStoryCommand(
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
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<AiStory> _storyRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GenerateStoryHandler(
        IStoryGeneratorService storyService,
        IRepository<User> userRepo,
        IRepository<AiStory> storyRepo,
        IUnitOfWork uow,
        ICurrentUserService currentUser)
    {
        _storyService = storyService;
        _userRepo = userRepo;
        _storyRepo = storyRepo;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<GenerateStoryResponse>> Handle(
        GenerateStoryCommand request, CancellationToken ct)
    {
        var users = await _userRepo.FindAsync(u => u.Id == _currentUser.UserId, ct);
        var user = users.FirstOrDefault();
        if (user is null)
            return Result<GenerateStoryResponse>.Failure("Useri nuk u gjet.");

        var result = await _storyService.GenerateAsync(
            user.Level,
            user.NativeLanguage,
            request.WordsToIntroduce,
            ct);

        var story = new AiStory
        {
            UserId = user.Id,
            TitleAlbanian = result.TitleAlbanian,
            BodyAlbanian = result.BodyAlbanian,
            BodyTranslated = result.BodyTranslated,
            VocabLevel = user.Level,
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