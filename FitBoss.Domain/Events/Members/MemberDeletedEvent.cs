using FitBoss.Domain.Common;
using FitBoss.Domain.Entities;

namespace Domain.Events.Members;
public class MemberDeletedEvent : BaseEvent
{
    public Member Member { get; set; }

    public MemberDeletedEvent(Member member)
    {
        Member = member;
    }
}
