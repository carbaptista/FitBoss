using Application.Features.Trainers.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Trainers;
using FitBoss.Application;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Trainers.Commands;
public class EditTrainerCommandHandlerTests
{
    private readonly Mock<ILogger<CreateTrainerCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public EditTrainerCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
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

        var editTrainer = CreateEditTrainer(createdTrainerResult.Data.Id, trainer.CreatorId);

        var editCommand = new EditTrainerCommand(editTrainer);
        var editHandler = new EditTrainerCommandHandler(new Mock<ILogger<EditTrainerCommandHandler>>().Object, _context);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().Be(createdTrainerResult.Data.Id);
        result.Data.UpdatedBy.Should().NotBeNull();

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(trainer.Name);
        result.Data.Name.Should().BeSameAs(editTrainer.Name);

        result.Data.Email.Should().NotBeNullOrEmpty();
        result.Data.Email.Should().NotBeSameAs(trainer.Email);
        result.Data.Email.Should().BeSameAs(editTrainer.Email);
    }

    private EditTrainerModel CreateEditTrainer(Guid id, Guid creatorId)
    {
        return new EditTrainerModel()
        {
            Id = id,
            UpdatedBy = Guid.NewGuid(),
            Name = "new name",
            Email = "test2@email.com"
        };
    }
}
