using Application.Features.Employees.Commands;
using Application.IntegrationTests.Context;
using Domain.Request_Models.Employee;
using FitBoss.Application;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Employees.Commands;
public class EditEmployeeCommandHandlerTests
{
    private readonly Mock<ILogger<CreateEmployeeCommandHandler>> _logger;
    private readonly Mock<ILogger<EditEmployeeCommandHandler>> _logger2;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public EditEmployeeCommandHandlerTests()
    {
        _logger = new();
        _logger2 = new();
        _context = new TestContextFactory().Create();
        _userManager = new UserManagerFactory().Create<BaseEntity>();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
    {
        var creatorId = Guid.NewGuid().ToString();
        var email = "test@email.com";
        var name = "name lastname";
        var userName = "name";

        var manager = Person.Create<Employee>(name, userName, email, creatorId);
        manager.Type = null;
        await _context.Employees.AddAsync(manager);
        await _context.SaveChangesAsync();

        var editManager = CreateEditEmployee(manager.Id, creatorId);

        var editCommand = new EditEmployeeCommand(editManager);
        var editHandler = new EditEmployeeCommandHandler(_logger2.Object, _context, _userManager.Object);

        _userManager.Setup(x => x.FindByIdAsync(manager.Id)).ReturnsAsync(new Employee() { Id = manager.Id });
        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<BaseEntity>())).ReturnsAsync(new List<string>());
        
        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().Be(manager.Id);
        result.Data.UpdatedBy.Should().NotBeNull();

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(name);
        result.Data.Name.Should().BeSameAs(editManager.Name);

        result.Data.Email.Should().NotBeNullOrEmpty();
        result.Data.Email.Should().NotBeSameAs(email);
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
