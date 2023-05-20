using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetMembersByNameQuery(string Name) : IRequest<Result<List<Member>>>;

public class GetMembersByNameQueryHandler : IRequestHandler<GetMembersByNameQuery, Result<List<Member>>>
{
    private readonly IApplicationDbContext _context;

    public GetMembersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Member>>> Handle(GetMembersByNameQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        if(members.Count == 0)
            return await Result<List<Member>>.FailureAsync($"No members found with name containing {request.Name}");

        return await Result<List<Member>>.SuccessAsync(members);
    }
}