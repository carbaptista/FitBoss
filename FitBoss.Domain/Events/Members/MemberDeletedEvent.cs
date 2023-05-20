using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Members;
public class MemberDeletedEvent : BaseEvent
{
    public Trainer Member { get; set; }

    public MemberDeletedEvent(Trainer member)
    {
        Member = member;
    }
}
