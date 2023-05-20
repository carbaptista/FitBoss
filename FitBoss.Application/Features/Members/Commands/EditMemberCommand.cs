using Domain.Events.Members;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record EditMemberCommand(EditMemberModel Member) : IRequest<Result<Member>>;

public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand, Result<Member>>
{
    private readonly ILogger<EditMemberCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public EditMemberCommandHandler(ILogger<EditMemberCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Member>> Handle(EditMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.Member.Id);
        if (member is null)
            return await Result<Member>.FailureAsync("Member not found");

        var updated = member.Update(request.Member);
        if (!updated)
        {
            _logger.LogError($"Error updating member with Id {member.Id} - {DateTime.UtcNow}");
            return await Result<Member>.FailureAsync("There was an error updating the member. Please try again");
        }

        try
        {
            _context.Members.Update(member);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Member>.FailureAsync("This email has already been registered");
            }
        }

        member.AddDomainEvent(new MemberUpdatedEvent(member));
        return await Result<Member>.SuccessAsync(member, "Member updated");
    }
}