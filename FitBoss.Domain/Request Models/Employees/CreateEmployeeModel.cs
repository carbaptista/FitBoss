using Domain.Enums;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Employee;

/// <summary>
/// Model for employee registration
/// </summary>
public class CreateEmployeeModel
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only letters are allowed")]
    [DefaultValue("new employee")]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 6)]
    [RegularExpression("^[a-zA-Z.]*$", ErrorMessage = "Only letters and periods with no spaces are allowed")]
    [DefaultValue("new.employee")]
    public string UserName { get; set; } = default!;
    
    [DefaultValue("8fe59047-f6fc-4066-a3f7-2c73cb9c063c")]
    public string CreatorId { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    [DefaultValue("newemp@test.com")]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [StringLength(256, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
    [DefaultValue("password")]
    public string Password { get; set; } = "";
}
