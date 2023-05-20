using Domain.Request_Models.Receptionists;
using Domain.Request_Models.Trainers;

namespace FitBoss.Domain.Entities;
public class Receptionist : Employee
{
    public decimal Salary
    {
        get
        {
            return BaseSalary * (decimal)1.1;
        }
    }

    public bool Update(EditReceptionistModel data)
    {
        Name = data.Name;
        Email = data.Email;
        UpdatedBy = data.UpdatedBy;
        UpdatedDate = DateTime.UtcNow;

        return true;
    }
}
