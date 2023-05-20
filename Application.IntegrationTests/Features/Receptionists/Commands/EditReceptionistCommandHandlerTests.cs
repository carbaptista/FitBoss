using Application.Features.Receptionists.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Receptionists;
using FitBoss.Application;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Receptionists.Commands;
public class EditReceptionistCommandHandlerTests
{
    private readonly Mock<ILogger<CreateReceptionistCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public EditReceptionistCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
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

        var editReceptionist = CreateEditReceptionist(createdReceptionistResult.Data.Id, receptionist.CreatorId);

        var editCommand = new EditReceptionistCommand(editReceptionist);
        var editHandler = new EditReceptionistCommandHandler(new Mock<ILogger<EditReceptionistCommandHandler>>().Object, _context);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().Be(createdReceptionistResult.Data.Id);
        result.Data.UpdatedBy.Should().NotBeNull();

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(receptionist.Name);
        result.Data.Name.Should().BeSameAs(editReceptionist.Name);

        result.Data.Email.Should().NotBeNullOrEmpty();
        result.Data.Email.Should().NotBeSameAs(receptionist.Email);
        result.Data.Email.Should().BeSameAs(editReceptionist.Email);
    }

    private EditReceptionistModel CreateEditReceptionist(Guid id, Guid creatorId)
    {
        return new EditReceptionistModel()
        {
            Id = id,
            UpdatedBy = Guid.NewGuid(),
            Name = "new name",
            Email = "test2@email.com"
        };
    }
}
