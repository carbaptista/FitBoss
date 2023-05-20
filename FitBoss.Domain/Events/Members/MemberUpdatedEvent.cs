using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Members;
public class MemberUpdatedEvent : BaseEvent
{
    public Trainer Member { get; set; }

    public MemberUpdatedEvent(Trainer member)
    {
        Member = member;
    }
}
