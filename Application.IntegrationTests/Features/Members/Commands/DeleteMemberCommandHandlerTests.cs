using Application.IntegrationTests.Context;
using Domain.Dtos;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace Application.IntegrationTests.Features.Members.Commands;
public class DeleteMemberCommandHandlerTests
{
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _logger;
    private readonly Mock<ILogger<DeleteMemberCommandHandler>> _logger2;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public DeleteMemberCommandHandlerTests()
    {
        _logger = new();
        _logger2 = new();
        _context = new TestContextFactory().Create();
        _userManager = new UserManagerFactory().Create<BaseEntity>();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccessResult_WhenValid()
    {
        var createdMemberResult = await CreateFreshMember();

        _userManager.Setup(x => x.DeleteAsync(It.IsAny<BaseEntity>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => new Member { });

        var deleteCommand = new DeleteMemberCommand(createdMemberResult.Data.Id);
        var deleteHandler = new DeleteMemberCommandHandler(_logger2.Object, _context, _userManager.Object);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_Should_ReturnFailureResult_WhenMemberNotFound()
    {
        var createdMemberResult = await CreateFreshMember();

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var deleteCommand = new DeleteMemberCommand(Guid.NewGuid().ToString());
        var deleteHandler = new DeleteMemberCommandHandler(_logger2.Object, _context, _userManager.Object);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<MemberDto>> CreateFreshMember()
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

        var createCommand = new CreateMemberCommand(member);
        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context, _userManager.Object);

        var createdMemberResult = await createHandler.Handle(createCommand, default);

        return createdMemberResult;
    }
}
