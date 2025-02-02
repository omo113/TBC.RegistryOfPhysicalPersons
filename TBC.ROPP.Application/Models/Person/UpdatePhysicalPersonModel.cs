using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Application.Models.Person;

public record UpdatePhysicalPersonModel(string Name, string LastName, string NameGe, string LastNameGe, Gender Gender, DateTimeOffset BirthDate, int CityId, PhoneNumberDto[] PhoneNumbers);