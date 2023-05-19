using FitBoss.Domain.Common;

namespace FitBoss.Domain.Interfaces;
public interface IDomainEventDispatcher
{
    Task DispatchAndClearEvents(IEnumerable<BaseEntity> entitiesWithEvents);
}
