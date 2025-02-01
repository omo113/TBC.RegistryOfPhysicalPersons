using TBC.ROPP.Domain.Shared;

namespace TBC.ROPP.Domain.Abstractions
{
    public abstract class AggregateRoot : Entity, IHasDomainEvent
    {
        private int Version { get; set; }

        private List<DomainEvent> DomainEvents { get; } = new List<DomainEvent>();

        public IReadOnlyList<DomainEvent> PendingDomainEvents()
        {
            return DomainEvents.Where(x => !x.IsPublished).ToList();
        }

        protected virtual void Raise(DomainEvent @event)
        {
            Version++;

            @event.UId = UId;
            @event.Version = Version;
            DomainEvents.Add(@event);
        }
    }
}