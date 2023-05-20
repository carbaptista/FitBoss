using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Trainers;
public class TrainerDeletedEvent : BaseEvent
{
    public Trainer Trainer { get; set; }

    public TrainerDeletedEvent(Trainer trainer)
    {
        Trainer = trainer;
    }
}
