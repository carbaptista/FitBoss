using Application.Features.Receptionists.Commands;
using Application.Features.Receptionists.Queries;
using Domain.Request_Models.Receptionists;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class ReceptionistsController : ControllerBase
{
    private readonly IMediator _mediatr;

    public ReceptionistsController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [Route("receptionists")]
    public async Task<IActionResult> GetAll()
    {
        var command = new GetAllReceptionistsQuery();
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route("receptionists/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var command = new GetReceptionistsByNameQuery(name);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route("receptionists/create")]
    public async Task<IActionResult> Create(CreateReceptionistModel receptionist)
    {
        var command = new CreateReceptionistCommand(receptionist);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    [HttpPatch]
    [Route("receptionists/update")]
    public async Task<IActionResult> Update([FromBody] EditReceptionistModel receptionist)
    {
        var command = new EditReceptionistCommand(receptionist);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    [HttpDelete]
    [Route("receptionists/delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var command = new DeleteReceptionistCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
