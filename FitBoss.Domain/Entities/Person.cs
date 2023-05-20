using FitBoss.Domain.Common;

namespace FitBoss.Domain.Entities;
public class Person : BaseAuditableEntity
{
    public string Name { get; set; } = "";
    public string Email { get; set; } = "";

    public static T Create<T>(string name, string email, Guid CreatorId) where T : Person, new()
    {
        return new T
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            CreatedBy = CreatorId,
            CreatedDate = DateTime.UtcNow
        };
    }
}
