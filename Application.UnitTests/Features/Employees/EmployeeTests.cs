using Domain.Enums;
using Domain.Request_Models.Employee;
using FitBoss.Domain.Entities;
using FluentAssertions;

namespace Application.UnitTests.Features.Employees;
public class EmployeeTests
{
    [Fact]
    public void Create_Should_ReturnEmployee()
    {
        var name = "firstname lastname";
        var username = "username";
        var email = "test@email.com";
        var creatorId = Guid.NewGuid().ToString();

        var result = Person.Create<Employee>(name, username, email, creatorId);

        result.Should().NotBeNull();
        result.Name.Should().BeSameAs(name);
        result.Email.Should().BeSameAs(email);
        result.Id.Should().NotBeEmpty();
        result.CreatedBy.Should().Be(creatorId);
    }

    [Fact]
    public void Update_Should_ReturnNewValues()
    {
        var name = "firstname lastname";
        var username = "username";
        var email = "test@email.com";
        var creatorId = Guid.NewGuid().ToString();

        var employee = Person.Create<Employee>(name, username, email, creatorId);

        var editedEmployee = new EditEmployeeModel()
        {
            Name = "new name",
            Email = "test2@email.com",
            UpdatedBy = Guid.NewGuid().ToString(),
            Type = EmployeeType.Manager
        };

        employee.Update(editedEmployee);

        employee.Name.Should().NotBeNullOrEmpty();
        employee.Name.Should().NotBeSameAs(name);
        employee.CreatedBy.Should().Be(creatorId);
        employee.Should().NotBeNull();
    }
}
