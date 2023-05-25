using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetEmployeeByIdQuery(string Id) : IRequest<Result<Employee>>;

public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, Result<Employee>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Employee>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
    {
        var employee = await _context.Employees.FindAsync(request.Id);
        if (employee is null)
            return await Result<Employee>.FailureAsync("Employee not found");

        return await Result<Employee>.SuccessAsync(employee);
    }
}
