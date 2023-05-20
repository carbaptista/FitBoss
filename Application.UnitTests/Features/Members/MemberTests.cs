using FitBoss.Domain.Entities;
using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;

namespace Application.UnitTests.Features.Members;

public class MemberTests
{
    [Fact]
    public void Create_Should_ReturnMember()
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

        var member = Person.Create<Trainer>(name, email, creatorId);

        var editedMember = new EditMemberModel()
        {
            Name = "new name",
            SubscriptionType = SubscriptionType.Yearly,
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
            Gender = true,
            Weight = 70,
            Height = 175,
            Email = "test2@email.com",
            UpdatedBy = Guid.NewGuid(),
        };

        member.Update(editedMember);

        member.Name.Should().NotBeNullOrEmpty();
        member.Name.Should().NotBeSameAs(name);
        member.CreatedBy.Should().Be(creatorId);
        member.SubscriptionType.Should().Be(SubscriptionType.Yearly);
        member.DateOfBirth.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)));
        member.Gender.Should().BeTrue();
        member.Weight.Should().Be(70);
        member.Height.Should().Be(175);
        member.Should().NotBeNull();
    }
}