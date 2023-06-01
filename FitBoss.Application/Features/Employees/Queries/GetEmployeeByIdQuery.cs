using Domain.Dtos;
using FitBoss.Application;
using MediatR;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetEmployeeByIdQuery(string Id) : IRequest<Result<EmployeeDto>>;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<EmployeeDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.Id);

        if (employee is null)
            return await Result<EmployeeDto>.FailureAsync("Employee not found");

        var employeeDto = employee.GetDto();

        return await Result<EmployeeDto>.SuccessAsync(employeeDto);
    }
}
