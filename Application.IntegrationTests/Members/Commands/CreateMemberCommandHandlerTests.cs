using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Members.Commands;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public CreateMemberCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Create_Should_ReturnSuccessResult_WhenValid()
    {
        var member = new CreateMemberModel()
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command = new CreateMemberCommand(member);
        var handler = new CreateMemberCommandHandler(_logger.Object, _context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().BeSameAs("Member created");
        result.Data.Name.Should().BeSameAs(member.Name);
        result.Data.Email.Should().BeSameAs(member.Email);
        result.Data.CreatedBy.Should().Be(member.CreatorId);
    }

    [Fact]
    public async Task Create_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        var handler = new CreateMemberCommandHandler(_logger.Object, _context);

        var member1 = new CreateMemberModel()
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command1 = new CreateMemberCommand(member1);
        await handler.Handle(command1, default);

        var member2 = new CreateMemberModel()
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command2 = new CreateMemberCommand(member2);
        var result = await handler.Handle(command2, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().Be("Email has already been registered");
    }
}
