using TBC.ROPP.Domain.Abstractions;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Domain.Entities;

public class PhoneNumber : Entity
{
    public PhoneNumberType PhoneNumberType { get; private set; }
    public string Number { get; private set; }

    private PhoneNumber()
    {

    }
    private PhoneNumber(PhoneNumberType numberType, string number)
    {
        UId = Guid.NewGuid();
        PhoneNumberType = numberType;
        Number = number;
    }

    public static PhoneNumber Create(PhoneNumberType numberType, string number)
    {
        return new PhoneNumber(numberType, number);
    }
}