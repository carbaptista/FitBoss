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
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public DeleteEmployeeCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
        _userManager = new();
    }

    [Fact]
    public async Task Delete_Should_ReturnSuccessResult_WhenValid()
    {
        var createdManagerResult = await CreateFreshManager();

        var deleteCommand = new DeleteEmployeeCommand(createdManagerResult.Data.Id);
        var deleteHandler = new DeleteEmployeerCommandHandler(new Mock<ILogger<DeleteEmployeerCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Delete_ShouldReturnFailureResult_WhenManagerNotFound()
    {
        var createdManagerResult = await CreateFreshManager();

        var deleteCommand = new DeleteEmployeeCommand(Guid.NewGuid().ToString());
        var deleteHandler = new DeleteEmployeerCommandHandler(new Mock<ILogger<DeleteEmployeerCommandHandler>>().Object, _context);

        var result = await deleteHandler.Handle(deleteCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private async Task<Result<Employee>> CreateFreshManager()
    {
        var employee = new CreateEmployeeModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateEmployeeCommand(employee);
        var createHandler = new CreateEmployeeCommandHandler(_logger.Object, _context, _userManager.Object);

        var createdEmployeeResult = await createHandler.Handle(createCommand, default);

        return createdEmployeeResult;
    }
}
