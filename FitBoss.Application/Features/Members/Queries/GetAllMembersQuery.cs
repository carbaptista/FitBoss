using Application.Extensions;
using Domain.Dtos;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared;

namespace FitBoss.Application.Features.Members.Queries;
public record GetAllMembersQuery(int page = 1, int pageSize = 30) : IRequest<PaginatedResult<MemberDto>>;

public class GetAllMembersQueryHandler : IRequestHandler<GetAllMembersQuery, PaginatedResult<MemberDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMembersQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedResult<MemberDto>> Handle(GetAllMembersQuery request, CancellationToken cancellationToken)
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
            .OrderBy(x => x.Name)
            .ToPaginatedListAsync(request.page, request.pageSize, cancellationToken);

        return members;
    }
}
