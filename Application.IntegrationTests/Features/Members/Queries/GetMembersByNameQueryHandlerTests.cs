using Application.IntegrationTests.Context;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Application;
using Microsoft.Extensions.Logging;
using Moq;
using FitBoss.Application.Features.Members.Queries;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Members.Queries;
public class GetMembersByNameQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetMembersByNameQueryHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetByName_Should_ReturnFailureResult_WhenNotFound()
    {
        var command = new GetMembersByNameQuery("name");
        var handler = new GetMembersByNameQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Data.Count.Should().Be(0);
    }
}
