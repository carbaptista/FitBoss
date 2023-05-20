using Domain.Events.Managers;
using FitBoss.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Managers.Commands;
public record DeleteManagerCommand(Guid Id) : IRequest<Result<bool>>;

public class DeleteManagerCommandHandler : IRequestHandler<DeleteManagerCommand, Result<bool>>
{
    private readonly ILogger<DeleteManagerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteManagerCommandHandler(ILogger<DeleteManagerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = await _context.Managers.FindAsync(request.Id);
        if (manager is null)
            return await Result<bool>.FailureAsync("Manager does not exist");

        _context.Managers.Remove(manager);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Manager with Id {manager.Id} deleted - {DateTime.UtcNow}");
        manager.AddDomainEvent(new ManagerDeletedEvent(manager));
        return await Result<bool>.SuccessAsync("Manager deleted");
    }
}