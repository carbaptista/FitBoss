using Application.Extensions;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetEmployeesByNameQuery(string Name, int page = 1, int pageSize = 30) : IRequest<PaginatedResult<Employee>>;

public class GetEmployeesByNameQueryHandler : IRequestHandler<GetEmployeesByNameQuery, PaginatedResult<Employee>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Employee>> Handle(GetEmployeesByNameQuery request, CancellationToken cancellationToken)
    {
        var employees = await _context.Employees
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return employees;
    }
}

