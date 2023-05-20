using Application.Features.Receptionists.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Receptionists.Queries;
public class GetReceptionistsByNameCommandHandler
{
    private readonly IApplicationDbContext _context;

    public GetReceptionistsByNameCommandHandler()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetByName_Should_ReturnFailureResult_WhenNotFound()
    {
        var command = new GetReceptionistsByNameQuery("name");
        var handler = new GetReceptionistsByNameQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
