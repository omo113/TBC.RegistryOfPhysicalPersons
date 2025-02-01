using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.NewFolder;

public class RelatedPhysicalPerson : Entity
{
    public PersonRelationshipType PersonRelationshipType { get; private set; }
    public int PhysicalPersonId { get; private set; }
    public PhysicalPerson PhysicalPerson { get; private set; }

    private RelatedPhysicalPerson()
    {

    }
    private RelatedPhysicalPerson(PersonRelationshipType personRelationshipType, int physicalPersonId)
    {
        UId = Guid.NewGuid();
        PersonRelationshipType = personRelationshipType;
        PhysicalPersonId = physicalPersonId;
    }

    public RelatedPhysicalPerson Create(PersonRelationshipType personRelationshipType, int physicalPersonId)
    {
        return new RelatedPhysicalPerson(personRelationshipType, physicalPersonId);
    }
}