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
using Shared;

namespace Application.IntegrationTests.Features.Employees.Commands;
public class DeleteEmployeeCommandHandlerTests
{
    private readonly Mock<ILogger<CreateEmployeeCommandHandler>> _logger;
    private readonly Mock<ILogger<DeleteEmployeerCommandHandler>> _logger2;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public DeleteEmployeeCommandHandlerTests()
    {
        _logger = new();
        _logger2 = new();
        _context = new TestContextFactory().Create();
        _userManager = new UserManagerFactory().Create<BaseEntity>();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccessResult_WhenValid()
    {
        var createdEmployeeResult = await CreateFreshEmployee();

        _userManager.Setup(x => x.DeleteAsync(It.IsAny<BaseEntity>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => new Employee { });

        var deleteCommand = new DeleteEmployeeCommand(createdEmployeeResult.Data.Id);
        var deleteHandler = new DeleteEmployeerCommandHandler(_logger2.Object, _context, _userManager.Object);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailureResult_WhenEmployeeNotFound()
    {
        var createdEmployeeResult = await CreateFreshEmployee();

        _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>()))
            .ReturnsAsync(() => null);

        var deleteCommand = new DeleteEmployeeCommand(Guid.NewGuid().ToString());
        var deleteHandler = new DeleteEmployeerCommandHandler(_logger2.Object, _context, _userManager.Object);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<Employee>> CreateFreshEmployee()
    {
        var employee = new CreateEmployeeModel
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

        var createCommand = new CreateEmployeeCommand(employee);
        var createHandler = new CreateEmployeeCommandHandler(_logger.Object, _context, _userManager.Object);

        var createdEmployeeResult = await createHandler.Handle(createCommand, default);

        return createdEmployeeResult;
    }
}
