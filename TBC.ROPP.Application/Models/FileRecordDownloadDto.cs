namespace TBC.ROPP.Application.Models;

public record FileRecordDownloadDto(Guid UId, string FileName, string Extension, MemoryStream Stream);