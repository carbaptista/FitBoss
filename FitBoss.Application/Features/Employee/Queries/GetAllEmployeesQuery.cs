using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Employees.Queries; 
public record GetAllEmployeesQuery : IRequest<Result<List<Employee>>>;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, Result<List<Employee>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllEmployeesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Employee>>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _context.Employees.ToListAsync();

        return await Result<List<Employee>>.SuccessAsync(employees);
    }
}
