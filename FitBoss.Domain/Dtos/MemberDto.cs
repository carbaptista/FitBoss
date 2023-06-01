using FitBoss.Domain.Enums;

namespace Domain.Dtos;
public class MemberDto
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";
    public int? Weight { get; set; }
    public int? Height { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public bool? Gender { get; set; }
    public SubscriptionType? SubscriptionType { get; set; }
}
