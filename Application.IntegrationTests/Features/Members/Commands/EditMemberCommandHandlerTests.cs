using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Members.Commands;
public class EditMemberCommandHandlerTests
{
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _logger;
    private readonly Mock<ILogger<EditMemberCommandHandler>> _logger2;
    private readonly IApplicationDbContext _context;
    private readonly Mock<UserManager<BaseEntity>> _userManager;

    public EditMemberCommandHandlerTests()
    {
        _logger = new();
        _logger2 = new();
        _context = new TestContextFactory().Create();
        _userManager = new UserManagerFactory().Create<BaseEntity>();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
    {
        var creatorId = Guid.NewGuid().ToString();
        var email = "test@email.com";
        var name = "name lastname";
        var userName = "name";

        var member = Person.Create<Member>(name, userName, email, creatorId);

        await _context.Members.AddAsync(member);
        await _context.SaveChangesAsync();

        var editMember = CreateEditMember(member.Id, creatorId);

        var editCommand = new EditMemberCommand(editMember);
        var editHandler = new EditMemberCommandHandler(_logger2.Object, _context, _userManager.Object);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().BeSameAs(member.Id);

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(name);
        result.Data.Name.Should().BeSameAs(editMember.Name);

        result.Data.Email.Should().NotBeNullOrEmpty();
        result.Data.Email.Should().NotBeSameAs(email);
        result.Data.Email.Should().BeSameAs(editMember.Email);

        result.Data.DateOfBirth.Should().NotBeNull();
        result.Data.Gender.Should().BeTrue();
        result.Data.Height.Should().Be(175);
        result.Data.Weight.Should().Be(70);
        result.Data.SubscriptionType.Should().Be(SubscriptionType.Monthly);
    }

    [Fact]
    public async Task Edit_ShouldReturnFailureResult_WhenEmailIsNotUnique()
    {
        var member1 = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var member2 = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test2@email.com",
            Name = "name lastname"
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<BaseEntity>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context, _userManager.Object);
        var createCommand1 = new CreateMemberCommand(member1);
        var createCommand2 = new CreateMemberCommand(member2);

        var createdMemberResult = await createHandler.Handle(createCommand1, default);
        await createHandler.Handle(createCommand2, default);

        var editedMember = CreateEditMember(createdMemberResult.Data.Id, member1.CreatorId);
        editedMember.Email = member2.Email;

        var editCommand = new EditMemberCommand(editedMember);
        var editHandler = new EditMemberCommandHandler(_logger2.Object, _context, _userManager.Object);

        _userManager.Setup(x => x.FindByEmailAsync(member2.Email)).ReturnsAsync(new Employee() { Email = member2.Email });

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Edit_ShouldReturnFailureResult_WhenMemberNotFound()
    {
        var member = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid().ToString(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        _userManager.Setup(x => x.CreateAsync(It.IsAny<BaseEntity>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success)
            .Verifiable();

        var createCommand = new CreateMemberCommand(member);
        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context, _userManager.Object);

        var createdMemberResult = await createHandler.Handle(createCommand, default);

        var editMember = CreateEditMember(Guid.NewGuid().ToString(), member.CreatorId);

        var editCommand = new EditMemberCommand(editMember);
        var editHandler = new EditMemberCommandHandler(_logger2.Object, _context, _userManager.Object);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private EditMemberModel CreateEditMember(string id, string creatorId)
    {
        return new EditMemberModel()
        {
            Id = id,
            UpdatedBy = Guid.NewGuid().ToString(),
            Name = "new name",
            Email = "test2@email.com",
            DateOfBirth = DateOnly.FromDateTime(DateTime.UtcNow.AddYears(-20)),
            Gender = true,
            Height = 175,
            Weight = 70,
            SubscriptionType = SubscriptionType.Monthly
        };
    }
}
