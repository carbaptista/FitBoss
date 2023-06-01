using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Employees;
public class EmployeeLoginModel
{
    /// <summary>
    /// Username - Usuário
    /// </summary>
    /// <example>testuser</example>
    [Required(ErrorMessage = "Username is required")]
    [DefaultValue("testuser")]
    public string UserName { get; set; } = "";

    /// <summary>
    /// Password - Senha
    /// </summary>
    /// <example>password</example>
    [DataType(DataType.Password)]
    [DefaultValue("password")]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
