using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Employees;
public class EmployeeDeletedEvent : BaseEvent
{
    public Employee Employee { get; set; }

    public EmployeeDeletedEvent(Employee employee)
    {
        Employee = employee;
    }
}
