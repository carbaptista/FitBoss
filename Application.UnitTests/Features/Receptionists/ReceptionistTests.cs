using Domain.Request_Models.Receptionists;
using Domain.Request_Models.Trainers;
using FitBoss.Domain.Entities;
using FluentAssertions;

namespace Application.UnitTests.Features.Receptionists;
public class ReceptionistTests
{
    [Fact]
    public void Create_Should_ReturnTrainer()
    {
        var name = "firstname lastname";
        var email = "test@email.com";
        var creatorId = Guid.NewGuid();

        var result = Person.Create<Receptionist>(name, email, creatorId);

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
        var creatorId = Guid.NewGuid();

        var receptionist = Person.Create<Receptionist>(name, email, creatorId);

        var editedManager = new EditReceptionistModel()
        {
            Name = "new name",
            Email = "test2@email.com",
            UpdatedBy = Guid.NewGuid(),
        };

        receptionist.Update(editedManager);

        receptionist.Name.Should().NotBeNullOrEmpty();
        receptionist.Name.Should().NotBeSameAs(name);
        receptionist.CreatedBy.Should().Be(creatorId);
        receptionist.Should().NotBeNull();
    }
}
