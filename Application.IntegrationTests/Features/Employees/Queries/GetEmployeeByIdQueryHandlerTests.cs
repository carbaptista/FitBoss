using Application.Features.Employees.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Employees.Queries;
public class GetEmployeeByIdQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByIdQueryHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetById_ShouldReturnFailureResult_WhenNotFound()
    {
        var command = new GetEmployeeByIdQuery(Guid.NewGuid().ToString());
        var handler = new GetEmployeeByIdQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
