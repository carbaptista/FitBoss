using Application.Features.Employees.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FitBoss.Application.Features.Members.Queries;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Employees.Queries;
public class GetEmployeesByNameQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesByNameQueryHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetByName_Should_ReturnFailureResult_WhenNotFound()
    {
        var command = new GetEmployeesByNameQuery("name");
        var handler = new GetEmployeesByNameQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Succeeded.Should().BeFalse();
        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
