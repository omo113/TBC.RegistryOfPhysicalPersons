using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Entities;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
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

    public int FileRecordId { get; private set; }
    public FileRecord? FileRecord { get; private set; }
    public int PhoneNumberId { get; private set; }
    public PhoneNumber PhoneNumber { get; private set; }
    private readonly List<RelatedPerson> _relatedPeopleList = [];
    public IEnumerable<RelatedPerson> RelatedPeopleList => _relatedPeopleList;

    private PhysicalPerson()
    {

    }

    private PhysicalPerson(string name, string lastName, Gender gender, string personalNumber, DateTimeOffset birthDate, int cityId, int fileRecordId, PhoneNumber phoneNumber)
    {
        UId = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Gender = gender;
        PersonalNumber = personalNumber;
        BirthDate = birthDate;
        CityId = cityId;
        FileRecordId = fileRecordId;
        PhoneNumber = phoneNumber;
    }

    public PhysicalPerson Create(string name, string lastName, Gender gender, string personalNumber, DateTimeOffset birthDate, int cityId, int fileRecordId, PhoneNumber phoneNumber)
    {
        return new PhysicalPerson(name, lastName, gender, personalNumber, birthDate, cityId, fileRecordId, phoneNumber);
    }
}