using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.File;
using TBC.ROPP.Application.Services.Abstractions;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.FileStorage.Abstractions;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;

namespace TBC.ROPP.Application.Services;

public class FileStorageService(IUnitOfWork unitOfWork, IRepository<FileRecord> fileRecordRepository, IFileStorage fileStorage) : IFileStorageService
{
    public async Task<FileRecordDto> GetFileRecordAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await fileRecordRepository.Query(x => x.UId == id)
            .Select(x => new FileRecordDto(x.UId, x.Name, x.Extension))
            .FirstAsync(cancellationToken: cancellationToken);

        return data;
    }

    public async Task<FileRecordDownloadDto> GetFileAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var data = await fileRecordRepository.Query(x => x.UId == id)
            .FirstAsync(cancellationToken: cancellationToken);

        var stream = await fileStorage.DownloadS3FileAsync(data, cancellationToken);

        return new FileRecordDownloadDto(data.UId, data.Name, data.Extension, stream);
    }

    public async Task<FileRecordDto> UploadAsync(IFormFile file, CancellationToken cancellationToken = default)
    {
        var extension = Path.GetExtension(file.FileName);
        var name = Path.GetFileNameWithoutExtension(file.FileName);
        var fileRecord = new FileRecord(name, extension);

        await fileStorage.UploadS3FileAsync(file, fileRecord.UId, cancellationToken);
        await fileRecordRepository.Store(fileRecord);
        await unitOfWork.SaveAsync(cancellationToken);

        return new FileRecordDto(fileRecord.UId, fileRecord.Name, fileRecord.Extension);
    }
}