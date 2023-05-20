using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace Application.Features.Receptionists.Queries;
public record GetReceptionistsByNameQuery(string Name) : IRequest<Result<List<Receptionist>>>;

public class GetReceptionistsByNameQueryHandler : IRequestHandler<GetReceptionistsByNameQuery, Result<List<Receptionist>>>
{
    private readonly IApplicationDbContext _context;

    public GetReceptionistsByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Receptionist>>> Handle(GetReceptionistsByNameQuery request, CancellationToken cancellationToken)
    {
        var receptionists = await _context.Receptionists
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        if (receptionists.Count == 0)
            return await Result<List<Receptionist>>.FailureAsync($"No receptionists with name containing {request.Name} found");

        return await Result<List<Receptionist>>.SuccessAsync(receptionists);
    }
}
