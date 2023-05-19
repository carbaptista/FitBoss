using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitBoss.Application.Features.Members.Queries;
public record GetAllMembersQuery() : IRequest<List<Member>>;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, List<Member>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Member>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members.ToListAsync(cancellationToken);

        return members;
    }
}
