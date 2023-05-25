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

    [Required(ErrorMessage = "You must specify an employee type")]
    public EmployeeType Type { get; set; }

    public decimal BaseSalary { get; set; }
    public decimal SalaryModifier { get; set; }
    public string Branch { get; set; } = "";

    [Required(ErrorMessage = "A hire date is required")]
    public DateOnly HiredDate { get; set; }
}
