using Domain.Events.Trainers;
using Domain.Request_Models.Trainers;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Trainers.Commands;
public record EditTrainerCommand(EditTrainerModel Trainer) : IRequest<Result<Trainer>>;

public class EditTrainerCommandHandler : IRequestHandler<EditTrainerCommand, Result<Trainer>>
{
    private readonly ILogger<EditTrainerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public EditTrainerCommandHandler(ILogger<EditTrainerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Trainer>> Handle(EditTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = await _context.Trainers.FindAsync(request.Trainer.Id);
        if (trainer is null)
            return await Result<Trainer>.FailureAsync("Trainer not found");

        var updated = trainer.Update(request.Trainer);

        if (!updated)
            _logger.LogError($"Error updating member with Id {trainer.Id} - {DateTime.UtcNow}");

        try
        {
            _context.Trainers.Update(trainer);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Trainer>.FailureAsync("This email has already been registered");
            }
        }

        trainer.AddDomainEvent(new TrainerUpdatedEvent(trainer));
        return await Result<Trainer>.SuccessAsync(trainer, "Trainer updated");
    }
}
