using Microsoft.AspNetCore.Http;
using TBC.ROPP.Application.Models.File;

namespace TBC.ROPP.Application.Services.Abstractions;

public interface IFileStorageService
{
    Task<FileRecordDto> UploadAsync(IFormFile file, CancellationToken cancellationToken);
    Task<FileRecordDownloadDto> GetFileAsync(Guid id, CancellationToken cancellationToken);
}
