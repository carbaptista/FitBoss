using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Receptionists;
public class ReceptionistCreatedEvent : BaseEvent
{
    public Receptionist Receptionist { get; set; }

    public ReceptionistCreatedEvent(Receptionist receptionist)
    {
        Receptionist = receptionist;
    }
}
