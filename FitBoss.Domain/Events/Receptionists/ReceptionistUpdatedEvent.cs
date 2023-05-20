using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Receptionists;
public class ReceptionistUpdatedEvent : BaseEvent
{
    public Receptionist Receptionist { get; set; }

    public ReceptionistUpdatedEvent(Receptionist receptionist)
    {
        Receptionist = receptionist;
    }
}
