using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetAllMembersQuery : IRequest<Result<List<Trainer>>>;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, Result<List<Trainer>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Trainer>>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members.ToListAsync(cancellationToken);

        return await Result<List<Trainer>>.SuccessAsync(members);
    }
}
