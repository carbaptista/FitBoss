using Application.Interfaces;
using Domain.Request_Models.Employees;
using FitBoss.Domain.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly UserManager<BaseEntity> _userManager;
    private readonly IAuthenticationManager _authManager;
    public AuthenticationController(UserManager<BaseEntity> userManager, IAuthenticationManager authManager)
    {
        _userManager = userManager;
        _authManager = authManager;
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Authenticate([FromBody] EmployeeLoginModel user)
    {
        if (!await _authManager.ValidateUser(user))
            return Unauthorized();

        return Ok(new { Token = await _authManager.CreateToken() });
    }
}
