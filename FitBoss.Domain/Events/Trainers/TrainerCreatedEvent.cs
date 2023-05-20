using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Trainers;
public class TrainerCreatedEvent : BaseEvent
{
    public Trainer Trainer { get; set; }

    public TrainerCreatedEvent(Trainer trainer)
    {
        Trainer = trainer;
    }
}
