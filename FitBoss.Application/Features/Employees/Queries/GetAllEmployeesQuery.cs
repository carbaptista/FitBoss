using Application.Extensions;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetAllEmployeesQuery(int page = 1, int pageSize = 30) : IRequest<PaginatedResult<Employee>>;

public class GetAllEmployeesQueryHandler : IRequestHandler<GetAllEmployeesQuery, PaginatedResult<Employee>>
{
    private readonly IApplicationDbContext _context;

    public GetAllEmployeesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Employee>> Handle(GetAllEmployeesQuery request, CancellationToken cancellationToken)
    {
        var employees = await _context.Employees
            .OrderBy(x => x.Name)
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return employees;
    }
}
