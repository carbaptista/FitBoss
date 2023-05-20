using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetAllMembersQuery() : IRequest<Result<List<Member>>>;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, Result<List<Member>>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Member>>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members.ToListAsync(cancellationToken);

        return await Result<List<Member>>.SuccessAsync(members);
    }
}
