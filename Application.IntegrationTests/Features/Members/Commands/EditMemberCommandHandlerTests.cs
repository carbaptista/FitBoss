using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests.Features.Members.Commands;
public class EditMemberCommandHandlerTests
{
    private readonly Mock<ILogger<CreateMemberCommandHandler>> _logger;
    private readonly IApplicationDbContext _context;

    public EditMemberCommandHandlerTests()
    {
        _logger = new();
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task Edit_Should_ReturnSuccessResult_WhenValid()
    {
        var member = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateMemberCommand(member);
        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context);

        var createdMemberResult = await createHandler.Handle(createCommand, default);

        var editMember = CreateEditMember(createdMemberResult.Data.Id, member.CreatorId);

        var editCommand = new EditMemberCommand(editMember);
        var editHandler = new EditMemberCommandHandler(new Mock<ILogger<EditMemberCommandHandler>>().Object, _context);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeTrue();
        result.Messages[0].Should().NotBeNullOrEmpty();

        result.Data.Id.Should().Be(createdMemberResult.Data.Id);
        result.Data.CreatedBy.Should().Be(editMember.CreatedBy);
        result.Data.UpdatedBy.Should().NotBeNull();

        result.Data.Name.Should().NotBeNullOrEmpty();
        result.Data.Name.Should().NotBeSameAs(member.Name);
        result.Data.Name.Should().BeSameAs(editMember.Name);

        result.Data.Email.Should().NotBeNullOrEmpty();
        result.Data.Email.Should().NotBeSameAs(member.Email);
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
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var member2 = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test2@email.com",
            Name = "name lastname"
        };

        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context);
        var createCommand1 = new CreateMemberCommand(member1);
        var createCommand2 = new CreateMemberCommand(member2);

        var createdMemberResult = await createHandler.Handle(createCommand1, default);
        await createHandler.Handle(createCommand2, default);

        var editedMember = CreateEditMember(createdMemberResult.Data.Id, member1.CreatorId);
        editedMember.Email = member2.Email;

        var editCommand = new EditMemberCommand(editedMember);
        var editHandler = new EditMemberCommandHandler(new Mock<ILogger<EditMemberCommandHandler>>().Object, _context);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task Edit_ShouldReturnFailureResult_WhenMemberNotFound()
    {
        var member = new CreateMemberModel
        {
            CreatorId = Guid.NewGuid(),
            Email = "test@email.com",
            Name = "name lastname"
        };

        var createCommand = new CreateMemberCommand(member);
        var createHandler = new CreateMemberCommandHandler(_logger.Object, _context);

        var createdMemberResult = await createHandler.Handle(createCommand, default);

        var editMember = CreateEditMember(Guid.NewGuid(), member.CreatorId);

        var editCommand = new EditMemberCommand(editMember);
        var editHandler = new EditMemberCommandHandler(new Mock<ILogger<EditMemberCommandHandler>>().Object, _context);

        var result = await editHandler.Handle(editCommand, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }

    private EditMemberModel CreateEditMember(Guid id, Guid creatorId)
    {
        return new EditMemberModel()
        {
            Id = id,
            CreatedBy = creatorId,
            UpdatedBy = Guid.NewGuid(),
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
