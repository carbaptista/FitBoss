using Domain.Request_Models.Employees;
using Shared;

namespace Blazor.Interfaces;

public interface IAuthService
{
    Task<LoginResult> Login(EmployeeLoginModel loginModel);
    Task Logout();
}
