using Microsoft.AspNetCore.Http;
using TBC.ROPP.Domain.Entities;

namespace TBC.ROPP.Infrastructure.FileStorage.Abstractions;

public interface IFileStorage
{
    Task UploadFileAsync(IFormFile file, Guid uId, CancellationToken cancellationToken);
    Task<MemoryStream> DownloadFileAsync(FileRecord key, CancellationToken cancellationToken);
}