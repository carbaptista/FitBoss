using Application.Features.Employees.Commands;
using Application.IntegrationTests.Context;
using Domain.Enums;
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
        var employeeModel = CreateEmployee();

        var employee = Person.Create<Employee>(
            employeeModel.Name,
            employeeModel.UserName,
            employeeModel.Email,
            employeeModel.CreatorId);

        employee.Type = null;
        await _context.Employees.AddAsync(employee);
        await _context.SaveChangesAsync();

        var editManager = CreateEditEmployee(employee.Id, employeeModel.CreatorId);

        var editCommand = new EditEmployeeCommand(editManager);
        var editHandler = new EditEmployeeCommandHandler(_logger2.Object, _context, _userManager.Object);

        _userManager.Setup(x => x.FindByIdAsync(employee.Id)).ReturnsAsync(new Employee() { Id = employee.Id });
        _userManager.Setup(x => x.GetRolesAsync(It.IsAny<BaseEntity>())).ReturnsAsync(new List<string>());
        
        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().Be(employee.Id);

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(employeeModel.Name);
        result.Data.Name.Should().BeSameAs(editManager.Name);

        result.Data.UserName.Should().BeSameAs(employee.UserName);
        result.Data.Email.Should().BeSameAs(employee.Email);
        result.Data.HireDate.Should().NotBe(null);
        result.Data.Branch.Should().Be("Salvador");
        result.Data.SalaryModifier.Should().Be(1);
        result.Data.Type.Should().Be(EmployeeType.Gerente);
    }

    private CreateEmployeeModel CreateEmployee()
    {
        return new CreateEmployeeModel
        {
            Name = "name lastname",
            Email = "test@gmail.com",
            Password = "password",
            UserName = "username",
            CreatorId = Guid.NewGuid().ToString()
        };
    }

    private EditEmployeeModel CreateEditEmployee(string id, string creatorId)
    {
        return new EditEmployeeModel
        {
            Id = id,
            UpdatedBy = Guid.NewGuid().ToString(),
            Name = "new name",
            BaseSalary = 2000,
            Branch = "Salvador",
            HiredDate = DateOnly.FromDateTime(DateTime.UtcNow),
            SalaryModifier = (decimal)1,
            Type = EmployeeType.Gerente
        };
    }
}
