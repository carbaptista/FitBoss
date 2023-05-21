using Domain.Events.Employees;
using FitBoss.Application;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Employees.Commands;
public record DeleteEmployeeCommand(string Id) : IRequest<Result<bool>>;

public class DeleteEmployeerCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result<bool>>
{
    private readonly ILogger<DeleteEmployeerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteEmployeerCommandHandler(ILogger<DeleteEmployeerCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.Id);
        if (employee is null)
            return await Result<bool>.FailureAsync("Employee does not exist");

        _context.Employees.Remove(employee);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Employee with Id {employee.Id} deleted - {DateTime.UtcNow}");
        employee.AddDomainEvent(new EmployeeDeletedEvent(employee));
        return await Result<bool>.SuccessAsync("Employee deleted");
    }
}