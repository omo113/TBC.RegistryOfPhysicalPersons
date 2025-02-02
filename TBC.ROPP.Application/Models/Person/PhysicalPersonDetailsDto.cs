using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Application.Models.Person;

public class PhysicalPersonDetailsDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required DateTimeOffset BirthDate { get; set; }
    public required int CityId { get; set; }
    public required string City { get; set; }
    public required string PersonalNumber { get; set; }
    public required Guid? FileUid { get; set; }
    public required IEnumerable<PhoneNumberDto> PhoneNumbers { get; set; }
    public required IEnumerable<RelatedPersonDto> RelatedPerson { get; set; }
    public required DateTimeOffset CreateDate { get; set; }
    public required DateTimeOffset? LastChangeDate { get; set; }
}