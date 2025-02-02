using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;

namespace TBC.ROPP.Application.Models.Person;

public record CreatePhysicalPersonModel(string Name, string LastName, Gender Gender, string PersonalNumber, DateTimeOffset BirthDate, int CityId, PhoneNumberDto[] PhoneNumbers);

public record PhoneNumberDto(PhoneNumberType NumberType, string TelephoneNumber);
public static class PhysicalPersonHelpers
{
    public static PhysicalPerson CreatePhysicalPerson(CreatePhysicalPersonModel createPhysicalPersonModel, IEnumerable<PhoneNumber> phoneNumbers)
    {
        return PhysicalPerson.Create(createPhysicalPersonModel.Name, createPhysicalPersonModel.LastName,
        createPhysicalPersonModel.Gender, createPhysicalPersonModel.PersonalNumber, createPhysicalPersonModel.BirthDate,
            createPhysicalPersonModel.CityId, phoneNumbers);
    }
    public static PhysicalPersonsDto CreatePhysicalPersonDto(this PhysicalPerson person)
            => new()
            {
                Id = person.Id,
                Name = person.Name,
                LastName = person.LastName,
                Gender = person.Gender,
                BirthDate = person.BirthDate,
                City = person.City?.Name,
                PersonalNumber = person.PersonalNumber,
                CreateDate = person.CreateDate
            };
}
