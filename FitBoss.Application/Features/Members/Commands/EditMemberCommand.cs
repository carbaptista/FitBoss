using Domain.Events.Members;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record EditMemberCommand(EditMemberModel Member) : IRequest<Result<bool>>;

public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public EditMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(EditMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.Member.Id);
        if (member is null)
            return await Result<bool>.FailureAsync("Member not found");

        var updated = member.Update(request.Member);
        if (!updated)
            return await Result<bool>.FailureAsync("There was an error updating member. Please try again");

        _context.Members.Update(member);
        await _context.SaveChangesAsync();

        member.AddDomainEvent(new MemberUpdatedEvent(member));
        return await Result<bool>.SuccessAsync("Member updated");
    }
}