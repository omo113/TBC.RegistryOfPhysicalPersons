using Microsoft.AspNetCore.Http;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.FileStorage.Abstractions;

namespace TBC.ROPP.Infrastructure.FileStorage;

public class FileStorage : IFileStorage
{
    public Task UploadFileAsync(IFormFile file, Guid uId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<MemoryStream> DownloadFileAsync(FileRecord key, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}