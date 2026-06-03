using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;
using System.Text.Json;

namespace MesoShqip.Application.Features.AI.Commands;

public record GenerateQuizCommand(
    Guid ChildProfileId,
    IReadOnlyList<string> WeakWords
) : IRequest<Result<GenerateQuizResponse>>;

public record GenerateQuizResponse(
    Guid SessionId,
    IReadOnlyList<QuizQuestion> Questions
);

public class GenerateQuizHandler : IRequestHandler<GenerateQuizCommand, Result<GenerateQuizResponse>>
{
    private readonly IQuizGeneratorService _quizService;
    private readonly IRepository<ChildProfile> _childRepo;
    private readonly IRepository<QuizSession> _sessionRepo;
    private readonly IUnitOfWork _uow;

    public GenerateQuizHandler(
        IQuizGeneratorService quizService,
        IRepository<ChildProfile> childRepo,
        IRepository<QuizSession> sessionRepo,
        IUnitOfWork uow)
    {
        _quizService = quizService;
        _childRepo = childRepo;
        _sessionRepo = sessionRepo;
        _uow = uow;
    }

    public async Task<Result<GenerateQuizResponse>> Handle(
        GenerateQuizCommand request, CancellationToken ct)
    {
        var children = await _childRepo.FindAsync(c => c.Id == request.ChildProfileId, ct);
        var child = children.FirstOrDefault();
        if (child is null)
            return Result<GenerateQuizResponse>.Failure("Profili nuk u gjet.");

        var result = await _quizService.GenerateAsync(
            child.NativeLanguage,
            request.WeakWords,
            ct);

        var session = new QuizSession
        {
            ChildProfileId = child.Id,
            QuizType = "AI-Generated",
            TotalQuestions = result.Questions.Count,
            QuestionsJson = JsonSerializer.Serialize(result.Questions)
        };

        await _sessionRepo.AddAsync(session, ct);
        await _uow.SaveChangesAsync(ct);

        return Result<GenerateQuizResponse>.Success(new GenerateQuizResponse(
            session.Id,
            result.Questions));
    }
}