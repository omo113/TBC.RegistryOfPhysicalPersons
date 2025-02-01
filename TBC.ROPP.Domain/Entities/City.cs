using TBC.ROPP.Domain.Abstractions;

namespace TBC.ROPP.Domain.Entities;

public class City : Entity
{
    public string Name { get; set; }
    public string NameGe { get; set; }

    private City()
    {

    }
    private City(string name, string nameGe)
    {
        UId = Guid.NewGuid();
        Name = name;
        NameGe = nameGe;
    }

    public static City Create(string name, string nameGe)
    {
        return new City(name, nameGe);
    }
}