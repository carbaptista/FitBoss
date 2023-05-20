using Domain.Events.Members;
using MediatR;
using Microsoft.Extensions.Logging;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record DeleteMemberCommand(Guid id) : IRequest<Result<bool>>;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Result<bool>>
{
    private readonly ILogger<DeleteMemberCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteMemberCommandHandler(ILogger<DeleteMemberCommandHandler> logger, IApplicationDbContext context)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<Result<bool>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.id);
        if (member is null)
            return await Result<bool>.FailureAsync("Member does not exist.");

        _context.Members.Remove(member);
        await _context.SaveChangesAsync();

        _logger.LogInformation($"Member with Id {member.Id} deleted {DateTime.UtcNow}");
        member.AddDomainEvent(new MemberDeletedEvent(member));
        return await Result<bool>.SuccessAsync("Member deleted");
    }
}
