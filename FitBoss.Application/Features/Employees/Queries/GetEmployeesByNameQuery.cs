using Application.Extensions;
using Domain.Dtos;
using FitBoss.Application;
using MediatR;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetEmployeesByNameQuery(string Name, int page = 1, int pageSize = 30) : IRequest<PaginatedResult<EmployeeDto>>;

public class GetEmployeesByNameQueryHandler : IRequestHandler<GetEmployeesByNameQuery, PaginatedResult<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<EmployeeDto>> Handle(GetEmployeesByNameQuery request, CancellationToken cancellationToken)
    {
        var employees = await _context.Employees
            .Select(x => new EmployeeDto
            {
                Id = x.Id,
                Name = x.Name,
                UserName = x.UserName!,
                Email = x.Email!,
                Branch = x.Branch,
                HireDate = x.HiredDate,
                Type = x.Type,
                SalaryModifier = x.SalaryModifier
            })
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return employees;
    }
}

