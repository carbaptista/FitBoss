using Domain.Events.Managers;
using Domain.Request_Models.Managers;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Managers.Commands;
public record EditManagerCommand(EditManagerModel data) : IRequest<Result<Manager>>;

public class EditManagerCommandHandler : IRequestHandler<EditManagerCommand, Result<Manager>>
{
    private readonly ILogger<EditManagerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public EditManagerCommandHandler(ILogger<EditManagerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Manager>> Handle(EditManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = await _context.Managers.FindAsync(request.data.Id);
        if (manager is null)
            return await Result<Manager>.FailureAsync($"Manager not found");

        var updated = manager.Update(request.data);
        if (!updated)
        {
            _logger.LogError($"Error updating member with Id {manager.Id} - {DateTime.UtcNow}");
            return await Result<Manager>.FailureAsync($"There was an error updating the manager. Please try again");
        }

        try
        {
            _context.Managers.Update(manager);
            await _context.SaveChangesAsync();
        }
        catch(DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Manager>.FailureAsync("This email has already been registered");
            }
        }

        manager.AddDomainEvent(new ManagerUpdatedEvent(manager));
        return await Result<Manager>.SuccessAsync(manager, "Manager updated");
    }
}
