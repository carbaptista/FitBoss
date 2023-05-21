using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Employees;
public class EmployeeUpdatedEvent : BaseEvent
{
    public Employee Employee { get; set; }

    public EmployeeUpdatedEvent(Employee employee)
    {
        Employee = employee;
    }
}
