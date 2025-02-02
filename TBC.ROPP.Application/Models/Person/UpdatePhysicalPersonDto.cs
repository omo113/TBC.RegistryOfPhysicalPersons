using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Application.Models.Person;

public record UpdatePhysicalPersonDto(string Name, string LastName, string NameGe, string LastNameGe, Gender Gender, DateTimeOffset BirthDate, int CityId, PhoneNumberDto[] PhoneNumbers);