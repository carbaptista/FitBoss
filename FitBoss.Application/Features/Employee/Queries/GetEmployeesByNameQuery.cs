using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Employees.Queries;
public record GetEmployeesByNameQuery(string Name) : IRequest<Result<List<Employee>>>;

public class GetEmployeesByNameQueryHandler : IRequestHandler<GetEmployeesByNameQuery, Result<List<Employee>>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Employee>>> Handle(GetEmployeesByNameQuery request, CancellationToken cancellationToken)
    {
        var employees = await _context.Employees
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        if (employees.Count == 0)
            return await Result<List<Employee>>.FailureAsync($"No employees found with name containing {request.Name}");

        return await Result<List<Employee>>.SuccessAsync(employees);
    }
}

