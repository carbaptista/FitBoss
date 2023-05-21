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
public record CreateEmployeeCommand(CreateEmployeeModel Manager) : IRequest<Result<Employee>>;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Employee>>
{
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public CreateEmployeeCommandHandler(ILogger<CreateEmployeeCommandHandler> logger,
                                        IApplicationDbContext context,
                                        UserManager<BaseEntity> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result<Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = Person.Create<Employee>(request.Manager.Name, request.Manager.UserName, request.Manager.Email, request.Manager.CreatorId);

        try
        {
            var result = await _userManager.CreateAsync(employee);

            if (!result.Succeeded)
            {
                var response = await Result<Employee>.FailureAsync("There was an error creating the employee. Please try again");
                _logger.LogError($"Error creating employee: {response.Exception.Message} - {DateTime.UtcNow}");
                return response;
            }
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Employee>.FailureAsync("This email has already been registered");
            }
        }

        employee.AddDomainEvent(new EmployeeCreatedEvent(employee));
        return await Result<Employee>.SuccessAsync(employee, "Employee created");
    }
}
