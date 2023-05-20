using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetMembersByNameQuery(string Name) : IRequest<Result<List<Trainer>>>;

public class GetMembersByNameQueryHandler : IRequestHandler<GetMembersByNameQuery, Result<List<Trainer>>>
{
    private readonly IApplicationDbContext _context;

    public GetMembersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<List<Trainer>>> Handle(GetMembersByNameQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToListAsync();

        if(members.Count == 0)
            return await Result<List<Trainer>>.FailureAsync($"No members with name containing {request.Name} found");

        return await Result<List<Trainer>>.SuccessAsync(members);
    }
}