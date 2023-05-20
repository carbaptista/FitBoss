using Application.Features.Trainers.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Trainers.Queries;
public class GetTrainersByNameCommandHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetTrainersByNameCommandHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetByName_Should_ReturnFailureResult_WhenNotFound()
    {
        var command = new GetTrainersByNameQuery("name");
        var handler = new GetTrainersByNameQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
