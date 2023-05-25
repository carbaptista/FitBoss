using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Employees.Queries;
public record GetEmployeeByUserNameQuery(string UserName) : IRequest<Result<Employee>>;

public class GetEmployeeByUserNameQueryHandler : IRequestHandler<GetEmployeeByUserNameQuery, Result<Employee>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByUserNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Employee>> Handle(GetEmployeeByUserNameQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Employees
            .Where(x => x.UserName == request.UserName)
            .FirstOrDefaultAsync();
        
        if (user is null)
            return await Result<Employee>.FailureAsync("Employee not found");

        return await Result<Employee>.SuccessAsync(user);
    }
}
