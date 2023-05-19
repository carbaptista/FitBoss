using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using MediatR;

namespace FitBoss.Domain.Entities;
public class Member : Person
{
    public int Number { get; private set; }
    public SubscriptionType SubscriptionType { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public bool? Gender { get; private set; }
    public int? Weight { get; private set; }
    public int? Height { get; private set; }

    public static Member Create(string name, Guid CreatorId, int number)
    {
        return new Member
        {
            Id = new Guid(),
            Name = name,
            Number = number,
            CreatedBy = CreatorId,
            CreatedDate = DateTime.UtcNow
        };
    }

    public bool Update(EditMemberModel member)
    {
        Name = member.Name;
        SubscriptionType = member.SubscriptionType;
        DateOfBirth = member.DateOfBirth;
        Gender = member.Gender;
        Weight = member.Weight;
        Height = member.Height;
        base.UpdatedBy = member.UpdatedBy;
        base.UpdatedDate = DateTime.UtcNow;

        return true;
    }
}