using Application.Features.Receptionists.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Receptionists;
using FitBoss.Application;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Receptionists.Commands;
public class CreateReceptionistCommandHandlerTests
{
    private readonly Mock<ILogger<CreateReceptionistCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public CreateReceptionistCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Create_Should_ReturnSuccessResult_WhenValid()
    {
        var receptionist = new CreateReceptionistModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command = new CreateReceptionistCommand(receptionist);
        var handler = new CreateReceptionistCommandHandler(_logger.Object, _context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
        result.Data.Name.Should().BeSameAs(receptionist.Name);
        result.Data.Email.Should().BeSameAs(receptionist.Email);
        result.Data.CreatedBy.Should().Be(receptionist.CreatorId);
    }

    [Fact]
    public async Task Create_Should_ReturnFailureResult_WhenEmailIsNotUnique()
    {
        var handler = new CreateReceptionistCommandHandler(_logger.Object, _context);

        var receptionist1 = new CreateReceptionistModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command1 = new CreateReceptionistCommand(receptionist1);
        await handler.Handle(command1, default);

        var receptionist2 = new CreateReceptionistModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var command2 = new CreateReceptionistCommand(receptionist2);
        var result = await handler.Handle(command2, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
