using TBC.ROPP.Domain.Abstractions.ValueObjects;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Entities;

public class RelatedPerson : ValueObjectEntity
{
    public PersonRelationshipType PersonRelationshipType { get; private set; }
    public int PhysicalPersonId { get; private set; }
    public PhysicalPerson PhysicalPerson { get; private set; }
    public int RelatedPhysicalPersonId { get; private set; }
    public PhysicalPerson RelatedPhysicalPerson { get; private set; }

    private RelatedPerson()
    {

    }
    private RelatedPerson(PersonRelationshipType personRelationshipType, int physicalPersonId, int relatedPhysicalPersonId)
    {
        UId = Guid.NewGuid();
        PersonRelationshipType = personRelationshipType;
        PhysicalPersonId = physicalPersonId;
    }

    public static RelatedPerson Create(PersonRelationshipType personRelationshipType, int physicalPersonId, int relatedPhysicalPersonId)
    {
        return new RelatedPerson(personRelationshipType, physicalPersonId, relatedPhysicalPersonId);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PersonRelationshipType;
        yield return PhysicalPersonId;
        yield return RelatedPhysicalPersonId;
    }
}