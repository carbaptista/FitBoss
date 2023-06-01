using Domain.Dtos;
using Domain.Events.Members;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record EditMemberCommand(EditMemberModel Member) : IRequest<Result<MemberDto>>;

public class EditMemberCommandHandler : IRequestHandler<EditMemberCommand, Result<MemberDto>>
{
    private readonly ILogger<EditMemberCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public EditMemberCommandHandler(
        ILogger<EditMemberCommandHandler> logger,
        IApplicationDbContext context,
        UserManager<BaseEntity> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result<MemberDto>> Handle(EditMemberCommand request, CancellationToken cancellationToken)
    {
        var member = await _context.Members.FindAsync(request.Member.Id);
        if (member is null)
            return await Result<MemberDto>.FailureAsync("Member not found");

        var exists = await _userManager.FindByEmailAsync(request.Member.Email);
        if (exists is not null)
            return await Result<MemberDto>.FailureAsync("This email has already been registered");

        var updated = member.Update(request.Member);
        if (!updated)
        {
            _logger.LogError($"Error updating member with Id {member.Id} - {DateTime.UtcNow}");
            return await Result<MemberDto>.FailureAsync("There was an error updating the member. Please try again");
        }

        _context.Members.Update(member);
        await _context.SaveChangesAsync();

        var memberDto = member.GetDto();

        member.AddDomainEvent(new MemberUpdatedEvent(member));
        return await Result<MemberDto>.SuccessAsync(memberDto, "Member updated");
    }
}