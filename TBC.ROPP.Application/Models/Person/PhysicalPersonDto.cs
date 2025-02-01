using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;
using TBC.ROPP.Domain.Entities;

namespace TBC.ROPP.Application.Models.Person;

public record PhysicalPersonDto(string Name, string LastName, string NameGe, string LastNameGe, Gender Gender, string PersonalNumber, DateTimeOffset DateOfBirth, int CityId, PhoneNumberType NumberType, string PhoneNumber);

public static class PhysicalPersonHelpers
{
    public static PhysicalPerson CreatePhysicalPerson(PhysicalPersonDto physicalPersonDto, PhoneNumber phoneNumber)
    {
        return PhysicalPerson.Create(physicalPersonDto.Name, physicalPersonDto.LastName, physicalPersonDto.NameGe,
        physicalPersonDto.LastNameGe, physicalPersonDto.Gender, physicalPersonDto.PersonalNumber, physicalPersonDto.DateOfBirth,
            physicalPersonDto.CityId, phoneNumber);
    }
}