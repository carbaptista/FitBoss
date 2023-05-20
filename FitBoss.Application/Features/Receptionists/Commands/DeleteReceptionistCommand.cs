using Application.Features.Trainers.Commands;
using Domain.Events.Receptionists;
using FitBoss.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Receptionists.Commands;
public record DeleteReceptionistCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteReceptionistCommandHandler : IRequestHandler<DeleteReceptionistCommand, Result<bool>>
{
    private readonly ILogger<DeleteReceptionistCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteReceptionistCommandHandler(ILogger<DeleteReceptionistCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteReceptionistCommand request, CancellationToken cancellationToken)
    {
        var receptionist = await _context.Receptionists.FindAsync(request.Id);
        if (receptionist is null)
            return await Result<bool>.FailureAsync("Receptionist does not exist");

        _context.Receptionists.Remove(receptionist);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Receptionist with Id {receptionist.Id} deleted - {DateTime.UtcNow}");
        receptionist.AddDomainEvent(new ReceptionistDeletedEvent(receptionist));
        return await Result<bool>.SuccessAsync("Receptionist deleted");
    }
}
