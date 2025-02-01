using TBC.ROPP.Shared;

namespace TBC.ROPP.Domain.Shared
{
    public interface IHasDomainEvent
    {
        IReadOnlyList<DomainEvent> PendingDomainEvents();
    }

    public abstract class DomainEvent
    {
        public Guid UId { get; set; }
        public DateTimeOffset DateOccurred { get; protected set; } = SystemDate.Now;
        public bool IsPublished { get; set; }
        public int Version { get; internal set; }

        public virtual void MarkAsPublished()
        {
            IsPublished = true;
        }
    }
}