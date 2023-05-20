using Domain.Request_Models.Managers;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Managers.Commands;
public record CreateManagerCommand(CreateManagerModel manager) : IRequest<Result<Manager>>;

public class CreateManagerCommandHandler : IRequestHandler<CreateManagerCommand, Result<Manager>>
{
    private readonly ILogger<CreateManagerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public CreateManagerCommandHandler(ILogger<CreateManagerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }


    public async Task<Result<Manager>> Handle(CreateManagerCommand request, CancellationToken cancellationToken)
    {
        var manager = Person.Create<Manager>(request.manager.Name, request.manager.Email, request.manager.CreatorId);

        try
        {
            await _context.Managers.AddAsync(manager);
            var result = await _context.SaveChangesAsync();

            if (result == 0)
            {
                var response = await Result<Manager>.FailureAsync("There was an error creating the manager. Please try again");
                _logger.LogError($"Error creating manager: {response.Exception.Message} - {DateTime.UtcNow}");
                return response;
            }
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Manager>.FailureAsync("This email has already been registered");
            }
        }

        return await Result<Manager>.SuccessAsync(manager, "Manager created");
    }
}
