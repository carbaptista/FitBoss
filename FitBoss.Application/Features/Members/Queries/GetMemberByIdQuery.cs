using Domain.Dtos;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using MediatR;
using Shared;

namespace Application.Features.Members.Queries;
public record GetMemberByIdQuery(string Id) : IRequest<Result<MemberDto>>;

public class GetMemberByIdQueryHandler : IRequestHandler<GetMemberByIdQuery, Result<MemberDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMemberByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<MemberDto>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.Id);
        if (member is null)
            return await Result<MemberDto>.FailureAsync("Member not found");

        var memberDto = member.GetDto();

        return await Result<MemberDto>.SuccessAsync(memberDto);
    }
}
