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
public record CreateTrainerCommand(CreateTrainerModel Trainer) : IRequest<Result<Trainer>>;

public class CreateTrainerCommandHandler : IRequestHandler<CreateTrainerCommand, Result<Trainer>>
{
    private readonly ILogger<CreateTrainerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public CreateTrainerCommandHandler(ILogger<CreateTrainerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Trainer>> Handle(CreateTrainerCommand request, CancellationToken cancellationToken)
    {
        var trainer = Person.Create<Trainer>(request.Trainer.Name, request.Trainer.Email, request.Trainer.CreatorId);

        try
        {
            await _context.Trainers.AddAsync(trainer);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                var response = await Result<Trainer>.FailureAsync("There was an error creating the member. Please try again");
                _logger.LogError($"Error creating trainer: {response.Exception.Message} - {DateTime.UtcNow}");
                return response;
            }
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Trainer>.FailureAsync("This email has already been registered");
            }
        }

        trainer.AddDomainEvent(new TrainerCreatedEvent(trainer));
        return await Result<Trainer>.SuccessAsync(trainer, "Trainer created");
    }
}
