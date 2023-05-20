using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;
namespace Domain.Events.Receptionists;
public class ReceptionistDeletedEvent : BaseEvent
{
    public Receptionist Receptionist { get; set; }

    public ReceptionistDeletedEvent(Receptionist receptionist)
    {
        Receptionist = receptionist;
    }
}
