using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.NewFolder;
using TBC.ROPP.Domain.Entities;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;

public class PhysicalPerson : AggregateRoot
{
    public string Name { get; private set; }
    public string LastName { get; private set; }
    public Gender Gender { get; private set; }
    public string PersonalNumber { get; private set; }
    public DateTimeOffset BirthDate { get; private set; }
    public int CityId { get; private set; }
    public City City { get; private set; }
    public string PhoneNumber { get; private set; }
    public int FileRecordId { get; private set; }
    public FileRecord FileRecord { get; private set; }
    private readonly List<RelatedPhysicalPerson> _relatedPeopleList = new();
    public IEnumerable<RelatedPhysicalPerson> RelatedPeopleList => _relatedPeopleList;

    private PhysicalPerson()
    {

    }

    private PhysicalPerson(string name, string lastName, Gender gender, string personalNumber, DateTimeOffset birthDate, int cityId, string phoneNumber, int fileRecordId)
    {
        UId = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Gender = gender;
        PersonalNumber = personalNumber;
        BirthDate = birthDate;
        CityId = cityId;
        PhoneNumber = phoneNumber;
        FileRecordId = fileRecordId;
    }

    public PhysicalPerson Create(string name, string lastName, Gender gender, string personalNumber,
        DateTimeOffset birthDate, int cityId, string phoneNumber, int fileRecordId)
    {
        return new PhysicalPerson(name, lastName, gender, personalNumber, birthDate, cityId, phoneNumber, fileRecordId);
    }
}