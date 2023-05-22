using Domain.Events.Members;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record DeleteMemberCommand(string id) : IRequest<Result<bool>>;

public class DeleteMemberCommandHandler : IRequestHandler<DeleteMemberCommand, Result<bool>>
{
    private readonly ILogger<DeleteMemberCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public DeleteMemberCommandHandler(
        ILogger<DeleteMemberCommandHandler> logger,
        IApplicationDbContext context,
        UserManager<BaseEntity> userManager)
    {
        _context = context;
        _logger = logger;
        _userManager = userManager;
    }

    public async Task<Result<bool>> Handle(DeleteMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _userManager.FindByIdAsync(request.id);
        if (member is null)
            return await Result<bool>.FailureAsync("Member does not exist.");

        var result = await _userManager.DeleteAsync(member);

        if (!result.Succeeded)
        {
            _logger.LogError($"There was an error deleting the member with Id {member.Id} - {DateTime.UtcNow}");
            return await Result<bool>.FailureAsync("There was an error deleting the member. Please try again");
        }

        _logger.LogInformation($"Member with Id {member.Id} deleted - {DateTime.UtcNow}");
        member.AddDomainEvent(new MemberDeletedEvent((Member)member));
        return await Result<bool>.SuccessAsync("Member deleted");
    }
}
