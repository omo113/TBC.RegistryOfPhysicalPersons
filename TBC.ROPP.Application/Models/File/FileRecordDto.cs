using TBC.ROPP.Domain.Entities;

namespace TBC.ROPP.Application.Models.File;

public record FileRecordDto(Guid Id, string Name, string Extension);

public static class FileRecordMappings
{
    public static FileRecordDto MapToDto(this FileRecord fileRecord)
    {
        return new FileRecordDto(fileRecord.UId, fileRecord.Name, fileRecord.Extension);
    }
}