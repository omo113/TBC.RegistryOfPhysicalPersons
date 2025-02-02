using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Application.Models.Person;

public class PhysicalPersonReport
{
    public string Name { get; set; }
    public string LastName { get; set; }
    public string PersonalNumber { get; set; }
    public List<RelatedPeopleCount> RelatedPeopleCounts { get; set; }
}

public class RelatedPeopleCount
{
    public PersonRelationshipType RelationshipType { get; set; }
    public int Count { get; set; }
}