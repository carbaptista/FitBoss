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
public class CreateEmployeeCommandHandlerTests
{
    private readonly Mock<ILogger<CreateEmployeeCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public CreateEmployeeCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
        _userManager = new UserManagerFactory().Create<BaseEntity>();
    }

    [Fact]
    public async Task Create_Should_ReturnSuccessResult_WhenValid()
    {
        var manager = new CreateEmployeeModel()
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname",
            UserName = "name",
            Password = "password"
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<BaseEntity>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        var command = new CreateEmployeeCommand(manager);
        var handler = new CreateEmployeeCommandHandler(_logger.Object, _context, _userManager.Object);

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
        var handler = new CreateEmployeeCommandHandler(_logger.Object, _context, _userManager.Object);

        var manager1 = new CreateEmployeeModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname",
            UserName = "name",
            Password = "password"
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<BaseEntity>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        var command1 = new CreateEmployeeCommand(manager1);
        await handler.Handle(command1, default);

        var manager2 = new CreateEmployeeModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname",
            UserName = "name",
            Password = "password"
        };

        _userManager.Setup(x => x.FindByEmailAsync(manager2.Email)).ReturnsAsync(new Employee() { Email = manager2.Email });

        var command2 = new CreateEmployeeCommand(manager2);
        var result = await handler.Handle(command2, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
