using Application.Features.Receptionists.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Receptionists;
using FitBoss.Application;
using FitBoss.Domain.Entities;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Shared;

namespace Application.IntegrationTests.Features.Receptionists.Commands;
public class DeleteReceptionistCommandHandlerTests
{
    private readonly Mock<ILogger<CreateReceptionistCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public DeleteReceptionistCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccessResult_WhenValid()
    {
        var createdReceptionistResult = await CreateFreshReceptionist();

        var deleteCommand = new DeleteReceptionistCommand(createdReceptionistResult.Data.Id);
        var deleteHandler = new DeleteReceptionistCommandHandler(new Mock<ILogger<DeleteReceptionistCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailureResult_WhenTrainerNotFound()
    {
        var createdReceptionistResult = await CreateFreshReceptionist();

        var deleteCommand = new DeleteReceptionistCommand(Guid.NewGuid());
        var deleteHandler = new DeleteReceptionistCommandHandler(new Mock<ILogger<DeleteReceptionistCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<Receptionist>> CreateFreshReceptionist()
    {
        var receptionist = new CreateReceptionistModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateReceptionistCommand(receptionist);
        var createHandler = new CreateReceptionistCommandHandler(_logger.Object, _context);

        var createdReceptionistResult = await createHandler.Handle(createCommand, default);

        return createdReceptionistResult;
    }
}
