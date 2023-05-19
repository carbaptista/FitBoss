using FitBoss.Domain.Common;

namespace FitBoss.Domain.Entities;
public class Person : BaseAuditableEntity
{
    public string Name { get; set; } = "";
}
