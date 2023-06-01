using Domain.Dtos;
using FitBoss.Application;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetEmployeeByUserNameQuery(string UserName) : IRequest<Result<EmployeeDto>>;

public class GetEmployeeByUserNameQueryHandler : IRequestHandler<GetEmployeeByUserNameQuery, Result<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByUserNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<EmployeeDto>> Handle(GetEmployeeByUserNameQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees
            .Where(x => x.UserName == request.UserName)
            .FirstOrDefaultAsync();

        if (employee is null)
            return await Result<EmployeeDto>.FailureAsync("Employee not found");

        var employeeDto = employee.GetDto();

        return await Result<EmployeeDto>.SuccessAsync(employeeDto);
    }
}
