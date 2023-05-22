using Domain.Request_Models.Employees;

namespace Application.Interfaces;
public interface IAuthenticationManager
{
    Task<bool> ValidateUser(EmployeeLoginModel employee);
    Task<string> CreateToken();
}
