using Domain.Events.Trainers;
using FitBoss.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Trainers.Commands;
public record DeleteTrainerCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteTrainerCommandHandler : IRequestHandler<DeleteTrainerCommand, Result<bool>>
{
    private readonly ILogger<DeleteTrainerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteTrainerCommandHandler(ILogger<DeleteTrainerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _context.Trainers.FindAsync(request.Id);
        if (trainer is null)
            return await Result<bool>.FailureAsync("Trainer does not exist");

        _context.Trainers.Remove(trainer);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Trainer with Id {trainer.Id} deleted - {DateTime.UtcNow}");
        trainer.AddDomainEvent(new TrainerDeletedEvent(trainer));
        return await Result<bool>.SuccessAsync("Trainer deleted");
    }
}
