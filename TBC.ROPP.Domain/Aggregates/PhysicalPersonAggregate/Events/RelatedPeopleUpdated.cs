using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Entities;
using TBC.ROPP.Domain.Shared;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Events;

public class RelatedPeopleUpdated : DomainEvent
{
    public List<RelatedPerson> RelatedPeople { get; set; }
}