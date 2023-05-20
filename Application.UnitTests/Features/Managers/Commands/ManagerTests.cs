using Domain.Request_Models.Managers;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;

namespace Application.UnitTests.Features.Managers.Commands;
public class ManagerTests
{
    [Fact]
    public void Create_Should_ReturnManager()
    {
        var name = "firstname lastname";
        var email = "test@email.com";
        var creatorId = new Guid("AAF20B83-687C-400C-A0CC-4A9CFF815321");

        var result = Person.Create<Manager>(name, email, creatorId);

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
        var email = "test@email.com";
        var creatorId = new Guid("AAF20B83-687C-400C-A0CC-4A9CFF815321");

        var manager = Person.Create<Manager>(name, email, creatorId);

        var editedManager = new EditManagerModel()
        {
            Name = "new name",
            Email = "test2@email.com",
            UpdatedBy = Guid.NewGuid(),
        };

        manager.Update(editedManager);

        manager.Name.Should().NotBeNullOrEmpty();
        manager.Name.Should().NotBeSameAs(name);
        manager.CreatedBy.Should().Be(creatorId);
        manager.Should().NotBeNull();
    }
}
