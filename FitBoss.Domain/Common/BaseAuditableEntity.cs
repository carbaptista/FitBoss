using FitBoss.Domain.Interfaces;

namespace FitBoss.Domain.Common;
public class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedDate { get; set; }
    public string? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
