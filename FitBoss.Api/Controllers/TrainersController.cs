using Application.Features.Trainers.Commands;
using Application.Features.Trainers.Queries;
using Domain.Request_Models.Trainers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class TrainersController : ControllerBase
{
    private readonly IMediator _mediatr;

    public TrainersController(IMediator mediatr)
    {
        _mediatr = mediatr;
    }

    [HttpGet]
    [Route("trainers")]
    public async Task<IActionResult> GetAll()
    {
        var command = new GetAllTrainersQuery();
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    [HttpGet]
    [Route("trainers/{name}")]
    public async Task<IActionResult> GetByName(string name)
    {
        var command = new GetTrainersByNameQuery(name);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    [HttpPost]
    [Route("trainers/create")]
    public async Task<IActionResult> Create(CreateTrainerModel trainer)
    {
        var command = new CreateTrainerCommand(trainer);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    [HttpPatch]
    [Route("trainers/update")]
    public async Task<IActionResult> Update([FromBody] EditTrainerModel trainer)
    {
        var command = new EditTrainerCommand(trainer);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    [HttpDelete]
    [Route("trainers/delete")]
    public async Task<IActionResult> Delete([FromBody] Guid id)
    {
        var command = new DeleteTrainerCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
