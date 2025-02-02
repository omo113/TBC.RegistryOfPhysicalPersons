using TBC.ROPP.Domain.Abstractions.ValueObjects;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Enums;

namespace TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;

public class PhoneNumber : ValueObject
{
    public int Id { get; private set; }
    public PhoneNumberType PhoneNumberType { get; private set; }
    public string Number { get; private set; }

    private PhoneNumber()
    {

    }
    private PhoneNumber(PhoneNumberType numberType, string number)
    {
        PhoneNumberType = numberType;
        Number = number;
    }

    public static PhoneNumber Create(PhoneNumberType numberType, string number)
    {
        return new PhoneNumber(numberType, number);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PhoneNumberType;
        yield return Number;
    }
}