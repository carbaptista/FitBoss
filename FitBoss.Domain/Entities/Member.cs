﻿using FitBoss.Domain.Enums;
using FitBoss.Domain.Request_Models.Members;
using MediatR;

namespace FitBoss.Domain.Entities;
public class Member : Person
{
    public SubscriptionType SubscriptionType { get; private set; }
    public DateOnly? DateOfBirth { get; private set; }
    public bool? Gender { get; private set; }
    public int? Weight { get; private set; }
    public int? Height { get; private set; }

    public static Member Create(string name, string email, Guid CreatorId)
    {
        return new Member
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            CreatedBy = CreatorId,
            CreatedDate = DateTime.UtcNow
        };
    }

    public bool Update(EditMemberModel newData)
    {
        Name = newData.Name;
        SubscriptionType = newData.SubscriptionType;
        DateOfBirth = newData.DateOfBirth;
        Gender = newData.Gender;
        Weight = newData.Weight;
        Height = newData.Height;
        Email = newData.Email;
        UpdatedBy = newData.UpdatedBy;
        UpdatedDate = DateTime.UtcNow;

        return true;
    }
}