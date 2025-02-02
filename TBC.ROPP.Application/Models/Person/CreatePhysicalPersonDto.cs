using TBC.ROPP.Application.Shared;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;

namespace TBC.ROPP.Application.Models.Person;

public record CreatePhysicalPersonDto(string Name, string LastName, string NameGe, string LastNameGe, Gender Gender, string PersonalNumber, DateTimeOffset BirthDate, int CityId, PhoneNumberDto[] PhoneNumbers);

public record PhoneNumberDto(PhoneNumberType NumberType, string TelephoneNumber);
public static class PhysicalPersonHelpers
{
    public static PhysicalPerson CreatePhysicalPerson(CreatePhysicalPersonDto createPhysicalPersonDto, IEnumerable<PhoneNumber> phoneNumbers)
    {
        return PhysicalPerson.Create(createPhysicalPersonDto.Name, createPhysicalPersonDto.LastName, createPhysicalPersonDto.NameGe,
        createPhysicalPersonDto.LastNameGe, createPhysicalPersonDto.Gender, createPhysicalPersonDto.PersonalNumber, createPhysicalPersonDto.BirthDate,
            createPhysicalPersonDto.CityId, phoneNumbers);
    }
    public static PhysicalPersonsDto CreatePhysicalPersonDto(this PhysicalPerson person)
            => new()
            {
                Id = person.Id,
                Name = CultureUtils.GetCurrentCultureIsGeorgian() ? person.NameGe : person.Name,
                LastName = CultureUtils.GetCurrentCultureIsGeorgian() ? person.LastNameGe : person.LastName,
                Gender = person.Gender,
                BirthDate = person.BirthDate,
                City = person.City.Name,
                PersonalNumber = person.PersonalNumber,
                CreateDate = person.CreateDate
            };
}
