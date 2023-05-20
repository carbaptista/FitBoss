using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Members;
public class MemberCreatedEvent : BaseEvent
{
    public Trainer Member { get; }

    public MemberCreatedEvent(Trainer member)
    {
        Member = member;
    }
}
