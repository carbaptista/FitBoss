using Domain.Dtos;
using Domain.Events.Members;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record CreateMemberCommand(CreateMemberModel Member) : IRequest<Result<MemberDto>>;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<MemberDto>>
{
    private readonly ILogger<CreateMemberCommandHandler> _logger;
    private readonly IApplicationDbContext _context;
    private readonly UserManager<BaseEntity> _userManager;

    public CreateMemberCommandHandler(
        ILogger<CreateMemberCommandHandler> logger,
        IApplicationDbContext context,
        UserManager<BaseEntity> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<Result<MemberDto>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = Person.Create<Member>(request.Member.Name, request.Member.UserName, request.Member.Email, request.Member.CreatorId);

        var exists = await _userManager.FindByEmailAsync(request.Member.Email);
        if (exists is not null)
            return await Result<MemberDto>.FailureAsync("This email has already been registered");

        var result = await _userManager.CreateAsync(member, request.Member.Password);

        if (!result.Succeeded)
        {
            List<string> errors = new();
            errors.Add("There was an error creating the member. Please try again");
            foreach (var error in result.Errors)
            {
                errors.Add(error.Description);
            }

            var response = await Result<MemberDto>.FailureAsync("There was an error creating the member. Please try again");
            _logger.LogError($"Error creating member: {response.Exception.Message} - {DateTime.UtcNow}");
            return response;
        }

        var memberDto = member.GetDto();

        member.AddDomainEvent(new MemberCreatedEvent(member));
        return await Result<MemberDto>.SuccessAsync(memberDto, "Member created");
    }
}