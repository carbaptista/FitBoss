using Domain.Events.Members;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared;

namespace FitBoss.Application.Features.Members.Commands;
public record CreateMemberCommand(CreateMemberModel member) : IRequest<Result<Member>>;

public class CreateMemberCommandHandler : IRequestHandler<CreateMemberCommand, Result<Member>>
{
    private readonly ILogger<CreateMemberCommandHandler> _logger;
    private readonly IApplicationDbContext _context;

    public CreateMemberCommandHandler(ILogger<CreateMemberCommandHandler> logger, IApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<Result<Member>> Handle(CreateMemberCommand request, CancellationToken cancellationToken)
    {
        var member = Member.Create(request.member.Name, request.member.Email, request.member.CreatorId);

        try
        {
            await _context.Members.AddAsync(member);
            var result = await _context.SaveChangesAsync(cancellationToken);

            if (result == 0)
            {
                var response = await Result<Member>.FailureAsync("There was an error creating the member. Please try again");
                _logger.LogError($"Error creating member: {response.Exception.Message} - {DateTime.UtcNow}");
                return response;
            }
        }
        catch (DbUpdateException e)
        {
            var innerException = e.InnerException as SqliteException;
            if (innerException != null && innerException.SqliteErrorCode == 19)
            {
                return await Result<Member>.FailureAsync("This email has already been registered");
            }
        }

        member.AddDomainEvent(new MemberCreatedEvent(member));
        return await Result<Member>.SuccessAsync(member, "Member created");
    }
}