using Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Domain.Request_Models.Employee;
public class EditEmployeeModel
{
    [Required(AllowEmptyStrings = false, ErrorMessage = "Id is required")]
    public string Id { get; set; } = "";

    [Required(AllowEmptyStrings = false, ErrorMessage = "Id of who's updating is required")]
    public string UpdatedBy { get; set; } = "";

    [Required]
    [DataType(DataType.Text)]
    [StringLength(50, ErrorMessage = "Name cannot be longer than {1}")]
    public string Name { get; set; } = "";

    [Required]
    [DataType(DataType.EmailAddress)]
    [StringLength(320, MinimumLength = 3)]
    public string Email { get; set; } = "";

    [Required(ErrorMessage = "You must specify an employee type")]
    public EmployeeType Type { get; set; }
}
