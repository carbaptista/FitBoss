using Domain.Request_Models.Managers;

namespace FitBoss.Domain.Entities;
public class Manager : Employee
{
    public decimal Salary
    {
        get
        {
            return BaseSalary * (decimal)1.3;
        }
    }

    public bool Update(EditManagerModel data)
    {
        Name = data.Name;
        Email = data.Email;
        UpdatedBy = data.UpdatedBy; 
        UpdatedDate = DateTime.UtcNow;

        return true;
    }
}
