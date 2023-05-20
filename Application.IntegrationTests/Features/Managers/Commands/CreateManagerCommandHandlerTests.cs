using FitBoss.Application.Features.Members.Commands;
using FitBoss.Application;
using Microsoft.Extensions.Logging;
using Moq;
using Application.Features.Managers.Commands;
using Application.IntegrationTests.Context;
using FitBoss.Domain.Request_Models.Members;
using Domain.Request_Models.Managers;
using FitBoss.Domain.Entities;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Managers.Commands;
public class CreateManagerCommandHandlerTests
{
    private readonly Mock<ILogger<CreateManagerCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public CreateManagerCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Create_Should_ReturnSuccessResult_WhenValid()
    {
        var manager = new CreateManagerModel()
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command = new CreateManagerCommand(manager);
        var handler = new CreateManagerCommandHandler(_logger.Object, _context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
        result.Data.Name.Should().BeSameAs(manager.Name);
        result.Data.Email.Should().BeSameAs(manager.Email);
        result.Data.CreatedBy.Should().Be(manager.CreatorId);
    }

    [Fact]
    public async Task Create_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        var handler = new CreateManagerCommandHandler(_logger.Object, _context);

        var manager1 = new CreateManagerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command1 = new CreateManagerCommand(manager1);
        await handler.Handle(command1, default);

        var manager2 = new CreateManagerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command2 = new CreateManagerCommand(manager2);
        var result = await handler.Handle(command2, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
