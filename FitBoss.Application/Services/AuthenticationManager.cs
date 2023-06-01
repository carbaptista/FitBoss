using Application.Interfaces;
using Domain.Request_Models.Employees;
using FitBoss.Domain.Common;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Services;
public class AuthenticationManager : IAuthenticationManager
{
    private readonly UserManager<BaseEntity> _userManager;
    private readonly IConfiguration _configuration;
    private BaseEntity? _user;

    public AuthenticationManager(UserManager<BaseEntity> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    public async Task<(bool, string?)> ValidateUser(EmployeeLoginModel employee)
    {
        _user = await _userManager.FindByNameAsync(employee.UserName);
        if (_user is null)
            return (false, null);

        return (await _userManager.CheckPasswordAsync(_user, employee.Password), _user.Id);
    }

    public async Task<string> CreateToken()
    {
        var signingCredentials = GetSigningCredentials();
        var claims = await GetClaims();
        var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
        return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
    }

    private SigningCredentials GetSigningCredentials()
    {
        var keyString = _configuration.GetSection("JwtSettings:SECRET").Value;
        var key = Encoding.UTF8.GetBytes(keyString!);
        var secret = new SymmetricSecurityKey(key);
        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    private async Task<List<Claim>> GetClaims()
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, _user?.UserName!)
        };
        var roles = await _userManager.GetRolesAsync(_user!);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }
        return claims;
    }

    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings.GetSection("validIssuer").Value,
                audience: jwtSettings.GetSection("validAudience").Value,
                claims: claims,
                expires:
                DateTime.Now.AddHours(Convert.ToDouble(jwtSettings.GetSection("expires").Value)),
                signingCredentials: signingCredentials
            );

        return tokenOptions;
    }
}
