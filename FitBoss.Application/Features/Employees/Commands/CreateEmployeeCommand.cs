using Domain.Events.Employees;
using Domain.Request_Models.Employee;
using FitBoss.Application;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared;

namespace Application.Features.Employees.Commands;
public record CreateEmployeeCommand(CreateEmployeeModel Employee) : IRequest<Result<Employee>>;

public class CreateEmployeeCommandHandler : IRequestHandler<CreateEmployeeCommand, Result<Employee>>
{
    private readonly ILogger<CreateEmployeeCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public CreateEmployeeCommandHandler(
        ILogger<CreateEmployeeCommandHandler> logger,
        IApplicationDbContext context,
        UserManager<BaseEntity> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result<Employee>> Handle(CreateEmployeeCommand request, CancellationToken cancellationToken)
    {
        var employee = Person.Create<Employee>(request.Employee.Name, request.Employee.UserName, request.Employee.Email, request.Employee.CreatorId);

        var exists = await _userManager.FindByEmailAsync(request.Employee.Email);
        if (exists is not null)
            return await Result<Employee>.FailureAsync("This email has already been registered");

        var result = await _userManager.CreateAsync(employee, request.Employee.Password);

        if (!result.Succeeded)
        {
            List<string> errors = new();
            errors.Add("There was an error creating the employee. Please try again");
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            var response = await Result<Employee>.FailureAsync(errors);
            _logger.LogError($"Error creating employee: {response.Exception.Message} - {DateTime.UtcNow}");
            return response;
        }

        employee.AddDomainEvent(new EmployeeCreatedEvent(employee));
        return await Result<Employee>.SuccessAsync(employee, "Employee created");
    }
}
