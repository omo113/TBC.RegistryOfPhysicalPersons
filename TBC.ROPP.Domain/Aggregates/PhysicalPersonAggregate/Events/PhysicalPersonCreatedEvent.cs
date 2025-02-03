using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Shared;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Events;

public class PhysicalPersonCreatedEvent : DomainEvent
{
    public required string PersonalNumber { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required DateTimeOffset BirthDate { get; set; }
    public required int CityId { get; set; }
}