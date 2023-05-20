using FitBoss.Application.Features.Members.Commands;
using FitBoss.Application.Features.Members.Queries;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FitBoss.Api.Controllers;

[ApiController]
public class MembersController : ControllerBase
{
    private readonly IMediator _mediatr;

    public MembersController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [Route("members")]
    public async Task<IActionResult> GetAll()
    {
        var command = new GetAllMembersQuery();
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route("members/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var command = new GetMembersByNameQuery(name);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route("members/create")]
    public async Task<IActionResult> Create(CreateMemberModel member)
    {
        var command = new CreateMemberCommand(member);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    [HttpPatch]
    [Route("members/update")]
    public async Task<IActionResult> Update([FromBody] EditMemberModel member)
    {
        var command = new EditMemberCommand(member);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    [HttpDelete]
    [Route("members/delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var command = new DeleteMemberCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
