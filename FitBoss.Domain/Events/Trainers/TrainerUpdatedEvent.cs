using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Trainers;
public class TrainerUpdatedEvent : BaseEvent
{
    public Trainer Trainer { get; set; }

    public TrainerUpdatedEvent(Trainer trainer)
    {
        Trainer = trainer;
    }
}
