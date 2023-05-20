using Application.Features.Managers.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Managers;
using FitBoss.Application;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Managers.Commands;
public class EditManagerCommandHandlerTests
{
    private readonly Mock<ILogger<CreateManagerCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public EditManagerCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
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

        var editManager = CreateEditManager(createdManagerResult.Data.Id, manager.CreatorId);

        var editCommand = new EditManagerCommand(editManager);
        var editHandler = new EditManagerCommandHandler(new Mock<ILogger<EditManagerCommandHandler>>().Object, _context);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().Be(createdManagerResult.Data.Id);
        result.Data.UpdatedBy.Should().NotBeNull();

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(manager.Name);
        result.Data.Name.Should().BeSameAs(editManager.Name);

        result.Data.Email.Should().NotBeNullOrEmpty();
        result.Data.Email.Should().NotBeSameAs(manager.Email);
        result.Data.Email.Should().BeSameAs(editManager.Email);
    }

    private EditManagerModel CreateEditManager(Guid id, Guid creatorId)
    {
        return new EditManagerModel()
        {
            Id = id,
            UpdatedBy = Guid.NewGuid(),
            Name = "new name",
            Email = "test2@email.com"
        };
    }
}
