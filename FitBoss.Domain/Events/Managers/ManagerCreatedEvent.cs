using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Managers;
public class ManagerCreatedEvent : BaseEvent
{
    public Manager Manager { get; set; }

    public ManagerCreatedEvent(Manager manager)
    {
        Manager = manager;
    }
}
