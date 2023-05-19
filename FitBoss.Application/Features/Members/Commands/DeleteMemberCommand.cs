using Domain.Events.Members;
using MediatR;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record DeleteMemberCommand(Guid id) : IRequest<Result<bool>>;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Result<bool>>
{
    private readonly IApplicationDbContext _context;

    public DeleteMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Result<bool>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.id);
        if (member is null)
            return await Result<bool>.FailureAsync("There was an error deleting the member. Please try again.");

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();


        member.AddDomainEvent(new MemberDeletedEvent(member));
        return await Result<bool>.SuccessAsync("Member deleted");
    }
}
