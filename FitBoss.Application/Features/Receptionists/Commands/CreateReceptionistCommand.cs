using Domain.Events.Receptionists;
using Domain.Events.Trainers;
using Domain.Request_Models.Receptionists;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Receptionists.Commands;
public record CreateReceptionistCommand(CreateReceptionistModel Receptionist) : IRequest<Result<Receptionist>>;

public class CreateReceptionistCommandHandler : IRequestHandler<CreateReceptionistCommand, Result<Receptionist>>
{
    private readonly ILogger<CreateReceptionistCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public CreateReceptionistCommandHandler(ILogger<CreateReceptionistCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public async Task<Result<Receptionist>> Handle(CreateReceptionistCommand request, CancellationToken cancellationToken)
    {
        var receptionist = Person.Create<Receptionist>(request.Receptionist.Name, request.Receptionist.Email, request.Receptionist.CreatorId);

        try
        {
            await _context.Receptionists.AddAsync(receptionist);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                var response = await Result<Receptionist>.FailureAsync("There was an error creating the receptionist. Please try again");
                _logger.LogError($"Error creating receptionist: {response.Exception.Message} - {DateTime.UtcNow}");
                return response;
            }
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Receptionist>.FailureAsync("This email has already been registered");
            }
        }

        receptionist.AddDomainEvent(new ReceptionistCreatedEvent(receptionist));
        return await Result<Receptionist>.SuccessAsync(receptionist, "Trainer created");
    }
}
