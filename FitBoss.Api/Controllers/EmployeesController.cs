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

    /// <summary>
    /// Gets all employees with pagination - Retorna todos os funcionários com paginação
    /// </summary>
    /// <param name="page"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("employees/{page:int?}")]
    public async Task<IActionResult> GetAll(int page)
    {
        var command = new GetAllEmployeesQuery(page);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    /// <summary>
    /// Gets employees by name with pagination - Retorna funcionários pelo nome com paginação
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="page">Page</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("employees/name/{name}/{page:int?}")]
    public async Task<IActionResult> GetByName(string name, int page = 1)
    {
        var command = new GetEmployeesByNameQuery(name, page);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }


    /// <summary>
    /// Gets an employee by Id - Retorna um funcionário pelo Id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("employees/id/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var command = new GetEmployeeByIdQuery(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    /// <summary>
    /// Gets an employee by username - Retorna um funcionário pelo nome de usuário
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("employees/username/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var command = new GetEmployeeByUserNameQuery(username);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    /// <summary>
    /// Creates an employee - Cria um funcionário
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    [HttpPost]
    [Route("employees/create")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Create(CreateEmployeeModel manager)
    {
        var command = new CreateEmployeeCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    /// <summary>
    /// Updates an employee - Atualiza um funcionário
    /// </summary>
    /// <param name="manager"></param>
    /// <returns></returns>
    [HttpPatch]
    [Route("employees/update")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Update([FromBody] EditEmployeeModel manager)
    {
        var command = new EditEmployeeCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }


    /// <summary>
    /// Deletes an employee - Deleta um funcionário
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Route("employees/delete")]
    [Authorize(Roles = "Gerente")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        var command = new DeleteEmployeeCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }

    [HttpPost]
    [Route("employees/createanonymous")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public async Task<IActionResult> CreateAnonymous(CreateEmployeeModel manager)
    {
        var command = new CreateEmployeeCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        EditEmployeeModel updateManager = new()
        {
            Id = result.Data.Id,
            UpdatedBy = result.Data.Id,
            Branch = "Salvador",
            HiredDate = DateOnly.FromDateTime(DateTime.UtcNow),
            Name = result.Data.Name,
            SalaryModifier = 1,
            Type = Domain.Enums.EmployeeType.Gerente
        };

        var command2 = new EditEmployeeCommand(updateManager);
        var result2 = await _mediatr.Send(command2);

        return Created("", result);
    }
}
