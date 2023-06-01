using Domain.Enums;

namespace Domain.Dtos;
public class EmployeeDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Branch { get; set; } = "";
    public EmployeeType? Type { get; set; }
    public decimal SalaryModifier { get; set; }
    public DateOnly HireDate { get; set; }
}
