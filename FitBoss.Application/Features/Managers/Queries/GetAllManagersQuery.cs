using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Managers.Queries; 
public record GetAllManagersQuery : IRequest<Result<List<Manager>>>;

public class GetAllManagersQueryHandler : IRequestHandler<GetAllManagersQuery, Result<List<Manager>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllManagersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Manager>>> Handle(GetAllManagersQuery request, CancellationToken cancellationToken)
    {
        var managers = await _context.Managers.ToListAsync();

        return await Result<List<Manager>>.SuccessAsync(managers);
    }
}
