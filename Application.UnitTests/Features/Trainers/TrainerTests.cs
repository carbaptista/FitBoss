using Domain.Request_Models.Trainers;
using FitBoss.Domain.Entities;
using FluentAssertions;

namespace Application.UnitTests.Features.Trainers;
public class TrainerTests
{
    [Fact]
    public void Create_Should_ReturnTrainer()
    {
        var name = "firstname lastname";
        var email = "test@email.com";
        var creatorId = Guid.NewGuid();

        var result = Person.Create<Trainer>(name, email, creatorId);

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

        var trainer = Person.Create<Trainer>(name, email, creatorId);

        var editedManager = new EditTrainerModel()
        {
            Name = "new name",
            Email = "test2@email.com",
            UpdatedBy = Guid.NewGuid(),
        };

        trainer.Update(editedManager);

        trainer.Name.Should().NotBeNullOrEmpty();
        trainer.Name.Should().NotBeSameAs(name);
        trainer.CreatedBy.Should().Be(creatorId);
        trainer.Should().NotBeNull();
    }
}
