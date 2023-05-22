using Application.Features.Employees.Commands;
using Application.Features.Employees.Queries;
using Domain.Request_Models.Employee;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly IMediator _mediatr;

    public EmployeesController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [Authorize]
    [Route("employees")]
    public async Task<IActionResult> GetAll()
    {
        var command = new GetAllEmployeesQuery();
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    [HttpGet]
    [Authorize]
    [Route("employees/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var command = new GetEmployeesByNameQuery(name);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    [HttpPost]
    [Route("employees/create")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Create(CreateEmployeeModel manager)
    {
        var command = new CreateEmployeeCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    [HttpPatch]
    [Route("employees/update")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Update([FromBody] EditEmployeeModel manager)
    {
        var command = new EditEmployeeCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    [HttpDelete]
    [Route("employees/delete")]
    [Authorize(Roles = "Manager")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        var command = new DeleteEmployeeCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
