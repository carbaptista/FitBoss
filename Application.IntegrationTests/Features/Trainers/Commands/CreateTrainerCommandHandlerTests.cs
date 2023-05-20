using Application.Features.Trainers.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Trainers;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Trainers.Commands;
public class CreateTrainerCommandHandlerTests
{
    private readonly Mock<ILogger<CreateTrainerCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public CreateTrainerCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Create_Should_ReturnSuccessResult_WhenValid()
    {
        var trainer = new CreateTrainerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command = new CreateTrainerCommand(trainer);
        var handler = new CreateTrainerCommandHandler(_logger.Object, _context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
        result.Data.Name.Should().BeSameAs(trainer.Name);
        result.Data.Email.Should().BeSameAs(trainer.Email);
        result.Data.CreatedBy.Should().Be(trainer.CreatorId);
    }

    [Fact]
    public async Task Create_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        var handler = new CreateTrainerCommandHandler(_logger.Object, _context);

        var trainer1 = new CreateTrainerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command1 = new CreateTrainerCommand(trainer1);
        await handler.Handle(command1, default);

        var trainer2 = new CreateTrainerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command2 = new CreateTrainerCommand(trainer2);
        var result = await handler.Handle(command2, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
