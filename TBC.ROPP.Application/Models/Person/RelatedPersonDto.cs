using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Application.Models.Person;

public record CreateRelatedPersonDto(PersonRelationshipType RelationshipType, int PersonId);

public record RelatedPersonDto(PersonRelationshipType RelationshipType, int PersonId, string Name, string LastName);