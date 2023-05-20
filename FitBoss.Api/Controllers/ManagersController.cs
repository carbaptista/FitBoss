﻿using Application.Features.Managers.Commands;
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

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    [HttpGet]
    [Route("managers/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var command = new GetManagersByNameQuery(name);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result);
    }

    [HttpPost]
    [Route("managers/create")]
    public async Task<IActionResult> Create(CreateManagerModel manager)
    {
        var command = new CreateManagerCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    [HttpPatch]
    [Route("managers/update")]
    public async Task<IActionResult> Update([FromBody] EditManagerModel manager)
    {
        var command = new EditManagerCommand(manager);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    [HttpDelete]
    [Route("managers/delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var command = new DeleteManagerCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
