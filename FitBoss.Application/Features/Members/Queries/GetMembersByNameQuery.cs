using Application.Extensions;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetMembersByNameQuery(string Name, int page = 1, int pageSize = 30) : IRequest<PaginatedResult<Member>>;

public class GetMembersByNameQueryHandler : IRequestHandler<GetMembersByNameQuery, PaginatedResult<Member>>
{
    private readonly IApplicationDbContext _context;

    public GetMembersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<Member>> Handle(GetMembersByNameQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return members;
    }
}