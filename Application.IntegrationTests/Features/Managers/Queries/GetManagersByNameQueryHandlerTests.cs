using Application.Features.Managers.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Queries;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Managers.Queries;
public class GetManagersByNameQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetManagersByNameQueryHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetByName_Should_ReturnFailureResult_WhenNotFound()
    {
        var command = new GetManagersByNameQuery("name");
        var handler = new GetManagersByNameQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
