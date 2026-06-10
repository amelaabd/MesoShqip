using MediatR;
using MesoShqip.Application.Common.Models;
using MesoShqip.Application.Interfaces;
using MesoShqip.Domain.Entities;
using MesoShqip.Domain.Interfaces;
using System.Text.Json;

namespace MesoShqip.Application.Features.AI.Commands;

public record GenerateQuizCommand(
    IReadOnlyList<string> WeakWords
) : IRequest<Result<GenerateQuizResponse>>;

public record GenerateQuizResponse(
    Guid SessionId,
    IReadOnlyList<QuizQuestion> Questions
);

public class GenerateQuizHandler : IRequestHandler<GenerateQuizCommand, Result<GenerateQuizResponse>>
{
    private readonly IQuizGeneratorService _quizService;
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<QuizSession> _sessionRepo;
    private readonly IUnitOfWork _uow;
    private readonly ICurrentUserService _currentUser;

    public GenerateQuizHandler(
        IQuizGeneratorService quizService,
        IRepository<User> userRepo,
        IRepository<QuizSession> sessionRepo,
        IUnitOfWork uow,
        ICurrentUserService currentUser)
    {
        _quizService = quizService;
        _userRepo = userRepo;
        _sessionRepo = sessionRepo;
        _uow = uow;
        _currentUser = currentUser;
    }

    public async Task<Result<GenerateQuizResponse>> Handle(
        GenerateQuizCommand request, CancellationToken ct)
    {
        var users = await _userRepo.FindAsync(u => u.Id == _currentUser.UserId, ct);
        var user = users.FirstOrDefault();
        if (user is null)
            return Result<GenerateQuizResponse>.Failure("Useri nuk u gjet.");

        var result = await _quizService.GenerateAsync(
            user.NativeLanguage,
            request.WeakWords,
            ct);

        var session = new QuizSession
        {
            UserId = user.Id,
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