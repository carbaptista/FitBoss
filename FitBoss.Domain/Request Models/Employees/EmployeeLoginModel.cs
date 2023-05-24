using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Employees;
public class EmployeeLoginModel
{
    [Required(ErrorMessage = "Username is required")]
    public string UserName { get; set; } = "";

    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = "";
}
