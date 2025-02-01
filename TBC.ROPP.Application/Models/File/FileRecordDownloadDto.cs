namespace TBC.ROPP.Application.Models.File;

public record FileRecordDownloadDto(Guid UId, string FileName, string Extension, MemoryStream Stream);