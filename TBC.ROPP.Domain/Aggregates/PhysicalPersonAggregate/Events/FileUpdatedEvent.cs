using TBC.ROPP.Domain.Shared;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Events;

public class FileUpdatedEvent : DomainEvent
{
    public required int FileRecordId { get; set; }
}