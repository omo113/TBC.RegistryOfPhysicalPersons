using TBC.ROPP.Domain.Abstractions;

namespace TBC.ROPP.Domain.Entities;

public class FileRecord : Entity
{
    public string Name { get; private set; }
    public string Extension { get; private set; }


    public FileRecord(string name, string extension)
    {
        UId = Guid.NewGuid();
        Name = name;
        Extension = extension;
    }
}