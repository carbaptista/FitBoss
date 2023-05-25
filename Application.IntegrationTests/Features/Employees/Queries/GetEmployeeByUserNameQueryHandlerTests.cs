using Application.Features.Employees.Queries;
using Application.IntegrationTests.Context;
using FitBoss.Application;
using FluentAssertions;

namespace Application.IntegrationTests.Features.Employees.Queries;
public class GetEmployeeByUserNameQueryHandlerTests
{
    private readonly IApplicationDbContext _context;

    public GetEmployeeByUserNameQueryHandlerTests()
    {
        _context = new TestContextFactory().Create();
    }

    [Fact]
    public async Task GetByUserName_ShouldReturnFailureResult_WhenNotFount()
    {
        var command = new GetEmployeeByUserNameQuery("username");
        var handler = new GetEmployeeByUserNameQueryHandler(_context);

        var result = await handler.Handle(command, default);

        result.Messages[0].Should().NotBeNullOrEmpty();
    }
}
