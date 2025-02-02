using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Application.Models.Person;

public class PhysicalPersonsDto
{
    public required int Id { get; set; }
    public required string Name { get; set; }
    public required string LastName { get; set; }
    public required Gender Gender { get; set; }
    public required DateTimeOffset BirthDate { get; set; }
    public required string? City { get; set; }
    public required string PersonalNumber { get; set; }
    public required DateTimeOffset CreateDate { get; set; }
    public required Guid? FileRecordUid { get; set; }

}


