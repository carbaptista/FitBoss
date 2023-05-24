using FitBoss.Domain.Common;

namespace FitBoss.Domain.Entities;
public class Person : BaseAuditableEntity
{
    public string Name { get; set; } = "";

    public static T Create<T>(string name, string username, string email, string CreatorId) where T : Person, new()
    {
        return new T
        {
            Id = Guid.NewGuid().ToString(),
            Name = name,
            UserName = username,
            Email = email,
            CreatedBy = CreatorId,
            CreatedDate = DateTime.UtcNow
        };
    }
}
