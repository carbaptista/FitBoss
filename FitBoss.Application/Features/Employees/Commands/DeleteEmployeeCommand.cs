using Domain.Events.Employees;
using FitBoss.Application;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Employees.Commands;
public record DeleteEmployeeCommand(string Id) : IRequest<Result<bool>>;

public class DeleteEmployeerCommandHandler : IRequestHandler<DeleteEmployeeCommand, Result<bool>>
{
    private readonly ILogger<DeleteEmployeerCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public DeleteEmployeerCommandHandler(
        ILogger<DeleteEmployeerCommandHandler> logger,
        IApplicationDbContext context,
        UserManager<BaseEntity> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _userManager.FindByIdAsync(request.Id);
        if (employee is null)
            return await Result<bool>.FailureAsync("Employee does not exist");

        var result = await _userManager.DeleteAsync(employee);

        if (!result.Succeeded)
        {
            _logger.LogError($"There was an error deleting the employee with Id {employee.Id} - {DateTime.UtcNow}");
            return await Result<bool>.FailureAsync("There was an error deleting the employee. Please try again");
        }

        _logger.LogInformation($"Employee with Id {employee.Id} deleted - {DateTime.UtcNow}");
        employee.AddDomainEvent(new EmployeeDeletedEvent((Employee)employee));
        return await Result<bool>.SuccessAsync("Employee deleted");
    }
}