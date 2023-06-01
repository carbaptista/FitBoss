using Application.Extensions;
using Domain.Dtos;
using MediatR;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetMembersByNameQuery(string Name, int page = 1, int pageSize = 30) : IRequest<PaginatedResult<MemberDto>>;

public class GetMembersByNameQueryHandler : IRequestHandler<GetMembersByNameQuery, PaginatedResult<MemberDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMembersByNameQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<MemberDto>> Handle(GetMembersByNameQuery request, CancellationToken cancellationToken)
    {
        var members = await _context.Members
            .Select(x => new MemberDto
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email!,
                Gender = x.Gender,
                Height = x.Height,
                Weight = x.Weight,
                SubscriptionType = x.SubscriptionType,
                DateOfBirth = x.DateOfBirth
            })
            .Where(x => x.Name.ToLower().Contains(request.Name))
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return members;
    }
}