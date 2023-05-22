using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Members.Commands;

public class CreateMemberCommandHandlerTests
{
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public CreateMemberCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
        _userManager = new UserManagerFactory().Create<BaseEntity>();
    }

    [Fact]
    public async Task Create_Should_ReturnSuccessResult_WhenValid()
    {
        var member = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname",
            UserName = "name",
            Password = "password"
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<BaseEntity>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        var command = new CreateMemberCommand(member);
        var handler = new CreateMemberCommandHandler(_logger.Object, _context, _userManager.Object);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
        result.Data.Name.Should().BeSameAs(member.Name);
        result.Data.Email.Should().BeSameAs(member.Email);
        result.Data.CreatedBy.Should().Be(member.CreatorId);
    }

    [Fact]
    public async Task Create_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        var handler = new CreateMemberCommandHandler(_logger.Object, _context, _userManager.Object);

        var member1 = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname",
            UserName = "name",
            Password = "password"
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<BaseEntity>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        var command1 = new CreateMemberCommand(member1);
        await handler.Handle(command1, default);

        var member2 = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname",
            UserName = "name",
            Password = "password"
        };

        _userManager.Setup(x => x.FindByEmailAsync(member2.Email)).ReturnsAsync(new Employee() { Email = member2.Email });

        var command2 = new CreateMemberCommand(member2);
        var result = await handler.Handle(command2, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
