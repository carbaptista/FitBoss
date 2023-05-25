using Domain.Request_Models.Employees;

namespace Application.Interfaces;
public interface IAuthenticationManager
{
    Task<(bool, string?)> ValidateUser(EmployeeLoginModel employee);
    Task<string> CreateToken();
}
