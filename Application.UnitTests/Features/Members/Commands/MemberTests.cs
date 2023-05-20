using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.UnitTests.Features.Members.Commands;

public class MemberTests
{
    [Fact]
    public void Create_Should_ReturnMember()
    {
        var name = "firstname lastname";
        var email = "test@email.com";
        var creatorId = new Guid("AAF20B83-687C-400C-A0CC-4A9CFF815321");

        var result = Member.Create(name, email, creatorId);

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

        var member = Member.Create(name, email, creatorId);

        var editedMember = new EditMemberModel()
        {
            Name = "firstname lastname",
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
        member.CreatedBy.Should().Be(creatorId);
        member.SubscriptionType.Should().Be(SubscriptionType.Yearly);
        member.DateOfBirth.Should().Be(DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)));
        member.Gender.Should().BeTrue();
        member.Weight.Should().Be(70);
        member.Height.Should().Be(175);
        member.Should().NotBeNull();
    }
}