using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace Application.IntegrationTests.Features.Members.Commands;
public class DeleteMemberCommandHandlerTests
{
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteMemberCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccess_WhenValid()
    {
        var createdMemberResult = await CreateFreshMember();

        var deleteCommand = new DeleteMemberCommand(createdMemberResult.Data.Id);
        var deleteHandler = new DeleteMemberCommandHandler(new Mock<ILogger<DeleteMemberCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_Should_ReturnFailureResult_WhenMemberNotFound()
    {
        var createdMemberResult = await CreateFreshMember();

        var deleteCommand = new DeleteMemberCommand(Guid.NewGuid());
        var deleteHandler = new DeleteMemberCommandHandler(new Mock<ILogger<DeleteMemberCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<Member>> CreateFreshMember()
    {
        var member = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateMemberCommand(member);
        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context);

        var createdMemberResult = await createHandler.Handle(createCommand, default);

        return createdMemberResult;
    }
}
