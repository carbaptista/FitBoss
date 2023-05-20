using FitBoss.Domain.Interfaces;

namespace FitBoss.Domain.Common;
public class BaseAuditableEntity : BaseEntity, IAuditableEntity
{
    public Guid CreatedBy { get; set; }
    public DateTime CreatedDate { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? UpdatedDate { get; set; }
}
