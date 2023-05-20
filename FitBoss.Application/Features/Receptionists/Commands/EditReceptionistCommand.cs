using Domain.Events.Receptionists;
using Domain.Request_Models.Receptionists;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Receptionists.Commands;
public record EditReceptionistCommand(EditReceptionistModel Receptionist) : IRequest<Result<Receptionist>>;

public class EditReceptionistCommandHandler : IRequestHandler<EditReceptionistCommand, Result<Receptionist>>
{
    private readonly ILogger<EditReceptionistCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public EditReceptionistCommandHandler(ILogger<EditReceptionistCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Receptionist>> Handle(EditReceptionistCommand request, CancellationToken cancellationToken)
    {
        var receptionist = await _context.Receptionists.FindAsync(request.Receptionist.Id);
        if (receptionist is null)
            return await Result<Receptionist>.FailureAsync("Receptionist not found");

        var updated = receptionist.Update(request.Receptionist);

        if (!updated)
            _logger.LogError($"Error updating receptionist with Id {receptionist.Id} - {DateTime.UtcNow}");

        try
        {
            _context.Receptionists.Update(receptionist);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Receptionist>.FailureAsync("This email has already been registered");
            }
        }

        receptionist.AddDomainEvent(new ReceptionistUpdatedEvent(receptionist));
        return await Result<Receptionist>.SuccessAsync(receptionist, "Receptionist updated");
    }
}
