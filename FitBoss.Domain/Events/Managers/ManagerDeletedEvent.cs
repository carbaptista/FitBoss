using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Managers;
public class ManagerDeletedEvent : BaseEvent
{
    public Manager Manager { get; set; }

    public ManagerDeletedEvent(Manager manager)
    {
        Manager = manager;
    }
}
