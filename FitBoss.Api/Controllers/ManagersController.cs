using Application.Features.Managers.Commands;
using Application.Features.Managers.Queries;
using Domain.Request_Models.Managers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class ManagersController : ControllerBase
{
    private readonly IMediator _mediatr;

    public ManagersController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [Route("managers")]
    public async Task<IActionResult> GetAll()
    {
        var command = new GetAllManagersQuery();
        var result = await _mediatr.Send(command);

        return Ok(result);
    }

    [HttpGet]
    [Route("managers/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        return Ok();
    }

    [HttpPost]
    [Route("managers/create")]
    public async Task<IActionResult> Create(CreateManagerModel data)
    {
        var command = new CreateManagerCommand(data);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
        {
            return Problem(result.Messages[0]);
        }

        return Ok(result);
    }
}
