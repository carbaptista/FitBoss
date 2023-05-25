using Application.Features.Members.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Members.Queries;
public class GetMemberByIdQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetMemberByIdQueryHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetById_ShouldReturnFailureResult_WhenNotFound()
    {
        var command = new GetMemberByIdQuery(Guid.NewGuid().ToString());
        var handler = new GetMemberByIdQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
