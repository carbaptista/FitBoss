using Application.Features.Members.Queries;
using FitBoss.Application.Features.Members.Commands;
using FitBoss.Application.Features.Members.Queries;
using FitBoss.Domain.Request_Models.Members;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

    /// <summary>
    /// Gets all members with pagination - Retorna todos os alunos com paginação
    /// </summary>
    /// <param name="page">Page</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("members/{page:int?}")]
    public async Task<IActionResult> GetAll(int page = 1)
    {
        var command = new GetAllMembersQuery(page);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    /// <summary>
    /// Gets members by name with pagination - Retorn alunos pelo nome com paginação
    /// </summary>
    /// <param name="name">Name</param>
    /// <param name="page">Page</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("members/name/{name}/{page:int?}")]
    public async Task<IActionResult> GetByName(string name, int page = 1)
    {
        var command = new GetMembersByNameQuery(name, page);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    /// <summary>
    /// Gets a member by id - Retorna um membro pelo id
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet]
    [Authorize]
    [Route("members/id/{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var command = new GetMemberByIdQuery(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return NotFound(result.Messages);

        return Ok(result.Data);
    }

    /// <summary>
    /// Creates a member - Cria um membro
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize(Roles = "Gerente, Recepcionista")]
    [Route("members/create")]
    public async Task<IActionResult> Create(CreateMemberModel member)
    {
        var command = new CreateMemberCommand(member);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Created("", result);
    }

    /// <summary>
    /// Updates a member - Atualiza um membro
    /// </summary>
    /// <param name="member"></param>
    /// <returns></returns>
    [HttpPatch]
    [Authorize(Roles = "Gerente, Recepcionista")]
    [Route("members/update")]
    public async Task<IActionResult> Update([FromBody] EditMemberModel member)
    {
        var command = new EditMemberCommand(member);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok(result);
    }

    /// <summary>
    /// Deletes a member - Deleta um membro
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete]
    [Authorize(Roles = "Gerente")]
    [Route("members/delete")]
    public async Task<IActionResult> Delete([FromBody] string id)
    {
        var command = new DeleteMemberCommand(id);
        var result = await _mediatr.Send(command);

        if (!result.Succeeded)
            return Problem(result.Messages[0]);

        return Ok();
    }
}
