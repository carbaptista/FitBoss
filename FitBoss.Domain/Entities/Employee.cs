using Domain.Enums;
using Domain.Request_Models.Employee;

namespace FitBoss.Domain.Entities;
public class Employee : Person
{
    public DateOnly HiredDate { get; set; }
    public decimal BaseSalary { get; set; } = 2000;
    public decimal SalaryModifier { get; set; } = 1;
    public EmployeeType? Type { get; set; } = null;
    public string Branch { get; set; } = "";

    public bool Update(EditEmployeeModel data)
    {
        Name = data.Name;
        Email = data.Email;
        UpdatedBy = data.UpdatedBy;
        UpdatedDate = DateTime.UtcNow;
        Type = data.Type;

        return true;
    }
}
