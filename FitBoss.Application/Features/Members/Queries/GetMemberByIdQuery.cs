using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.Members.Queries;
public record GetMemberByIdQuery(string Id) : IRequest<Result<Member>>;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Result<Member>>
{
    private readonly IApplicationDbContext _context;

    public GetMemberByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<Member>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _context.Members.FindAsync(request.Id);
        if (user is null)
            return await Result<Member>.FailureAsync("Member not found");

        return await Result<Member>.SuccessAsync(user);
    }
}
