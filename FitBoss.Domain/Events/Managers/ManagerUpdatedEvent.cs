using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Events.Managers;
public class ManagerUpdatedEvent : BaseEvent
{
    public Manager Manager { get; set; }

    public ManagerUpdatedEvent(Manager manager)
    {
        Manager = manager;
    }
}
