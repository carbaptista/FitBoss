using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Members;
public class MemberCreatedEvent : BaseEvent
{
    public Member Member { get; }

    public MemberCreatedEvent(Member member)
    {
        Member = member;
    }
}
