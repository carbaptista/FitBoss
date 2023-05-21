using Application.Features.Employees.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Employee;
using FitBoss.Application;
using FitBoss.Domain.Common;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Employees.Commands;
public class EditEmployeeCommandHandlerTests
{
    private readonly Mock<ILogger<CreateEmployeeCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public EditEmployeeCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
        _userManager = new();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
    {
        var manager = new CreateEmployeeModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateEmployeeCommand(manager);
        var createHandler = new CreateEmployeeCommandHandler(_logger.Object, _context, _userManager.Object);

        var createdManagerResult = await createHandler.Handle(createCommand, default);

        var editManager = CreateEditEmployee(createdManagerResult.Data.Id, manager.CreatorId);

        var editCommand = new EditEmployeeCommand(editManager);
        var editHandler = new EditEmployeeCommandHandler(new Mock<ILogger<EditEmployeeCommandHandler>>().Object, _context, _userManager.Object);

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

    private EditEmployeeModel CreateEditEmployee(string id, string creatorId)
    {
        return new EditEmployeeModel()
        {
            Id = id,
            UpdatedBy = Guid.NewGuid().ToString(),
            Name = "new name",
            Email = "test2@email.com"
        };
    }
}
