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

namespace Application.Features.Managers.Queries;
public record GetManagersByNameQuery(string Name) : IRequest<Result<List<Manager>>>;

public class GetManagersByNameQueryHandler : IRequestHandler<GetManagersByNameQuery, Result<List<Manager>>>
{
    private readonly IApplicationDbContext _context;

    public GetManagersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Manager>>> Handle(GetManagersByNameQuery request, CancellationToken cancellationToken)
    {
        var managers = await _context.Managers
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        if (managers.Count == 0)
            return await Result<List<Manager>>.FailureAsync($"No managers found with name containing {request.Name}");

        return await Result<List<Manager>>.SuccessAsync(managers);
    }
}

