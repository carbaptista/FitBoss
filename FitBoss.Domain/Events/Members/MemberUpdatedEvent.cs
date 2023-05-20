using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Members;
public class MemberUpdatedEvent : BaseEvent
{
    public Member Member { get; set; }

    public MemberUpdatedEvent(Member member)
    {
        Member = member;
    }
}
