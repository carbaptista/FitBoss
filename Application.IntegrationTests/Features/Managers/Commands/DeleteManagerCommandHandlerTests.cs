using Application.Features.Managers.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Managers;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace Application.IntegrationTests.Features.Managers.Commands;
public class DeleteManagerCommandHandlerTests
{
    private readonly Mock<ILogger<CreateManagerCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteManagerCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccessResult_WhenValid()
    {
        var createdManagerResult = await CreateFreshManager();

        var deleteCommand = new DeleteManagerCommand(createdManagerResult.Data.Id);
        var deleteHandler = new DeleteManagerCommandHandler(new Mock<ILogger<DeleteManagerCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailureResult_WhenManagerNotFound()
    {
        var createdManagerResult = await CreateFreshManager();

        var deleteCommand = new DeleteManagerCommand(Guid.NewGuid());
        var deleteHandler = new DeleteManagerCommandHandler(new Mock<ILogger<DeleteManagerCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<Manager>> CreateFreshManager()
    {
        var manager = new CreateManagerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateManagerCommand(manager);
        var createHandler = new CreateManagerCommandHandler(_logger.Object, _context);

        var createdManagerResult = await createHandler.Handle(createCommand, default);

        return createdManagerResult;
    }
}
