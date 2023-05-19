using FitBoss.Domain.Request_Models.Members;
using MediatR;

namespace FitBoss.Application.Features.Members.Commands;
public record EditMemberCommand(EditMemberModel Member) : IRequest<bool>;

public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand, bool>
{
    private readonly IApplicationDbContext _context;

    public EditMemberCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(EditMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.Member.Id);
        if (member is null)
            return false;

        var updated = member.Update(request.Member);
        if (!updated)
            return false;

        _context.Members.Update(member);
        await _context.SaveChangesAsync();

        return true;
    }
}