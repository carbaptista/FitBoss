using Application.Extensions;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetAllMembersQuery(int page = 1, int pageSize = 30) : IRequest<PaginatedResult<Member>>;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, PaginatedResult<Member>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Member>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members
            .OrderBy(x => x.Name)
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return members;
    }
}
