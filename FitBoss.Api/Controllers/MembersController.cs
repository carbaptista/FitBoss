using Microsoft.AspNetCore.Mvc;
using MediatR;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Application.Features.Members.Queries;
using FitBoss.Domain.Entities;
using FitBoss.Domain.Request_Models;
using FitBoss.Domain.Request_Models.Members;

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
        var request = new GetAllMembersQuery();
        var result = await _mediatr.Send(request);

        if (!result.Succeeded)
        {
            return NotFound(result.Messages[0]);
        }

        return Ok(result.Data);
    }

    [HttpGet]
    [Route("members/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var request = new GetMembersByNameQuery(name);
        var result = await _mediatr.Send(request);

        if (!result.Succeeded)
        {
            return NotFound(result.Messages[0]);
        }

        return Ok(result.Data);
    }

    [HttpPost]
    [Route("members/create")]
    public async Task<IActionResult> Create(CreateMemberModel member)
    {
        var request = new CreateMemberCommand(member);
        var result = await _mediatr.Send(request);

        if (!result.Succeeded)
        {
            return Problem(result.Messages[0]);
        }

        return Ok(result);
    }

    [HttpPatch]
    [Route("members/update")]
    public async Task<IActionResult> Update([FromBody] EditMemberModel member)
    {
        var request = new EditMemberCommand(member);
        var result = await _mediatr.Send(request);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    [HttpDelete]
    [Route("members/delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var request = new DeleteMemberCommand(id);
        var result = await _mediatr.Send(request);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
