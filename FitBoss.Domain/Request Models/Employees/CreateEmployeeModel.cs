using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Employee;
public class CreateEmployeeModel
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Only letters are allowed")]
    public string Name { get; set; } = default!;

    [Required]
    [StringLength(50, MinimumLength = 6)]
    [RegularExpression("^[a-zA-Z.]*$", ErrorMessage = "Only letters and periods with no spaces are allowed")]
    public string UserName { get; set; } = default!;

    public string CreatorId { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    public string Email { get; set; } = "";

    [DataType(DataType.Password)]
    [StringLength(256, ErrorMessage = "Password must be at least 6 characters long.", MinimumLength = 6)]
    public string Password { get; set; } = "";
}
