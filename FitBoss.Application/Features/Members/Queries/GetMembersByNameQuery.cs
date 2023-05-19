using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FitBoss.Application.Features.Members.Queries;
public record GetMembersByNameQuery(string Name) : IRequest<List<Member>>;

public class GetMembersByNameQueryHandler : IRequestHandler<GetMembersByNameQuery, List<Member>>
{
    private readonly IApplicationDbContext _context;

    public GetMembersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Member>> Handle(GetMembersByNameQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        return members;
    }
}