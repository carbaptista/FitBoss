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
        var response = await _mediatr.Send(request);

        if (response is null)
        {
            return Problem();
        }

        return Ok(response);
    }

    [HttpGet]
    [Route("members/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var request = new GetMembersByNameQuery(name);
        var response = await _mediatr.Send(request);

        if (response is null)
        {
            return Problem();
        }

        return Ok(response);
    }

    [HttpPost]
    [Route("members/create")]
    public async Task<IActionResult> Create(CreateMemberModel member)
    {
        var request = new CreateMemberCommand(member);
        var response = await _mediatr.Send(request);

        if (!response.Succeeded)
        {
            return Problem(response.Messages[0]);
        }

        return Ok(response);
    }

    [HttpPatch]
    [Route("members/update")]
    public async Task<IActionResult> Update([FromBody] EditMemberModel member)
    {
        var request = new EditMemberCommand(member);
        var response = await _mediatr.Send(request);

        if (!response.Succeeded)
            return Problem(response.Messages[0]);

        return Ok(response);
    }

    [HttpDelete]
    [Route("members/delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var request = new DeleteMemberCommand(id);
        var response = await _mediatr.Send(request);

        if (!response.Succeeded)
            return Problem(response.Messages[0]);

        return Ok();
    }
}
