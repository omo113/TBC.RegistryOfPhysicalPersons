using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Entities;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Shared;

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
    private readonly List<PhoneNumber> _phoneNumbers = [];
    public IEnumerable<PhoneNumber> PhoneNumbers => _phoneNumbers;

    // ReSharper disable once CollectionNeverUpdated.Local
    private readonly List<RelatedPerson> _relatedPeopleAsOwner = new();
    public IEnumerable<RelatedPerson> RelatedPeopleAsOwner => _relatedPeopleAsOwner;

    private readonly List<RelatedPerson> _relatedPeopleList = [];
    public IEnumerable<RelatedPerson> RelatedPeopleList => _relatedPeopleList;

    private PhysicalPerson()
    {

    }

    private PhysicalPerson(string name, string lastName, Gender gender,
        string personalNumber, DateTimeOffset birthDate, int cityId, IEnumerable<PhoneNumber> phoneNumbers)
    {
        UId = Guid.NewGuid();
        Name = name;
        LastName = lastName;
        Gender = gender;
        PersonalNumber = personalNumber;
        BirthDate = birthDate;
        CityId = cityId;
        _phoneNumbers.AddRange(phoneNumbers);
        //todo event

    }

    public static PhysicalPerson Create(string name, string lastName, Gender gender, string personalNumber, DateTimeOffset birthDate, int cityId, IEnumerable<PhoneNumber> phoneNumbers)
    {
        return new PhysicalPerson(name, lastName, gender, personalNumber, birthDate, cityId, phoneNumbers);
    }

    public DomainResult<PhysicalPerson, DomainValidation> UpdateFields(string name, string lastName, Gender gender, DateTimeOffset birthDate, int cityId, List<PhoneNumber> phoneNumbers)
    {
        Name = name;
        LastName = lastName;
        Gender = gender;
        BirthDate = birthDate;
        CityId = cityId;


        var phoneNumbersToRemove = _phoneNumbers
            .Where(pn => !phoneNumbers.Contains(pn))
            .ToList();

        foreach (var phoneNumber in phoneNumbersToRemove)
        {
            _phoneNumbers.Remove(phoneNumber);
        }

        var phoneNumbersToAdd = phoneNumbers
            .Where(pn => !_phoneNumbers.Contains(pn))
            .ToList();

        _phoneNumbers.AddRange(phoneNumbersToAdd);
        LastChangeDate = SystemDate.Now;
        //todo event

        return new DomainResult<PhysicalPerson, DomainValidation>(this);
    }

    public DomainResult<PhysicalPerson, DomainValidation> UpdateRelatedPeople(List<RelatedPerson> relatedPeople)
    {
        var relatedPeopleToRemove = _relatedPeopleList
            .Where(rp => !relatedPeople.Contains(rp))
            .ToList();

        foreach (var relatedPerson in relatedPeopleToRemove)
        {
            _relatedPeopleList.Remove(relatedPerson);
        }

        var relatedPeopleToAdd = relatedPeople
            .Where(rp => !_relatedPeopleList.Contains(rp))
            .ToList();

        _relatedPeopleList.AddRange(relatedPeopleToAdd);
        LastChangeDate = SystemDate.Now;
        //todo event

        return new DomainResult<PhysicalPerson, DomainValidation>(this);
    }
    public DomainResult<PhysicalPerson, DomainValidation> UpdateFile(int fileRecordId)
    {
        FileRecordId = fileRecordId;
        LastChangeDate = SystemDate.Now;
        //todo event

        return new DomainResult<PhysicalPerson, DomainValidation>(this);
    }
}