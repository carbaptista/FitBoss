using Application.Features.Trainers.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Trainers;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace Application.IntegrationTests.Features.Trainers.Commands;
public class DeleteTrainerCommandHandlerTests
{
    private readonly Mock<ILogger<CreateTrainerCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteTrainerCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccessResult_WhenValid()
    {
        var createdTrainerResult = await CreateFreshTrainer();

        var deleteCommand = new DeleteTrainerCommand(createdTrainerResult.Data.Id);
        var deleteHandler = new DeleteTrainerCommandHandler(new Mock<ILogger<DeleteTrainerCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailureResult_WhenTrainerNotFound()
    {
        var createdTrainerResult = await CreateFreshTrainer();

        var deleteCommand = new DeleteTrainerCommand(Guid.NewGuid());
        var deleteHandler = new DeleteTrainerCommandHandler(new Mock<ILogger<DeleteTrainerCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<Trainer>> CreateFreshTrainer()
    {
        var trainer = new CreateTrainerModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateTrainerCommand(trainer);
        var createHandler = new CreateTrainerCommandHandler(_logger.Object, _context);

        var createdTrainerResult = await createHandler.Handle(createCommand, default);

        return createdTrainerResult;
    }
}
