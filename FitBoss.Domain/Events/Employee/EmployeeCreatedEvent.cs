using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Employees;
public class EmployeeCreatedEvent : BaseEvent
{
    public Employee Employee { get; set; }

    public EmployeeCreatedEvent(Employee employee)
    {
        Employee = employee;
    }
}
