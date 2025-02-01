using Microsoft.AspNetCore.Http;
using TBC.ROPP.Domain.Entities;

namespace TBC.ROPP.Infrastructure.FileStorage.Abstractions;

public interface IFileStorage
{
    Task UploadS3FileAsync(IFormFile file, Guid uId, CancellationToken cancellationToken);
    Task<MemoryStream> DownloadS3FileAsync(FileRecord key, CancellationToken cancellationToken);
}

public interface IS3Bucket
{
    Task EnsureBucketExistsAsync(string bucketName, CancellationToken cancellationToken);
}