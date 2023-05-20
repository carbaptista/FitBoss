using Domain.Request_Models.Trainers;

namespace FitBoss.Domain.Entities;
public class Trainer : Employee
{
    public decimal Salary
    {
        get
        {
            return BaseSalary * (decimal)1.2;
        }
    }

    public bool Update(EditTrainerModel data)
    {
        Name = data.Name;
        Email = data.Email;
        UpdatedBy = data.UpdatedBy;
        UpdatedDate = DateTime.UtcNow;

        return true;
    }
}
