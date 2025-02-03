using TBC.ROPP.Domain.Shared;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Events;

public class RelatedPeopleUpdated : DomainEvent
{
    public List<int> RelatedPeopleIds { get; set; }
}
