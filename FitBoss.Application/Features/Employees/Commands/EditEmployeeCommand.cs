using Application.Helpers;
using Domain.Events.Employees;
using Domain.Request_Models.Employee;
using FitBoss.Application;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Employees.Commands;
public record EditEmployeeCommand(EditEmployeeModel Employee) : IRequest<Result<Employee>>;

public class EditEmployeeCommandHandler : IRequestHandler<EditEmployeeCommand, Result<Employee>>
{
    private readonly ILogger<EditEmployeeCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public EditEmployeeCommandHandler(
        ILogger<EditEmployeeCommandHandler> logger,
        IApplicationDbContext context,
        UserManager<BaseEntity> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result<Employee>> Handle(EditEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.Employee.Id);
        if (employee is null)
            return await Result<Employee>.FailureAsync("Employee not found");

        var updated = employee.Update(request.Employee);
        if (!updated)
        {
            _logger.LogError($"Error updating member with Id {employee.Id} - {DateTime.UtcNow}");
            return await Result<Employee>.FailureAsync("There was an error updating the employee. Please try again");
        }

        _context.Employees.Update(employee);
        var saveResult = await _context.SaveChangesAsync(cancellationToken);

        if(saveResult == 0)
            return await Result<Employee>.FailureAsync("There was an error updating the employee. Please try again");

        if (employee.Type is not null)
        {
            var user = await _userManager.FindByIdAsync(employee.Id);
            var currentRoles = await _userManager.GetRolesAsync(user!);
            await _userManager.RemoveFromRolesAsync(user!, currentRoles.ToArray());
            await _userManager.AddToRoleAsync(user!, employee.Type.ToString()!);
        }

        employee.AddDomainEvent(new EmployeeUpdatedEvent(employee));
        return await Result<Employee>.SuccessAsync(employee, "Employee updated");
    }
}
