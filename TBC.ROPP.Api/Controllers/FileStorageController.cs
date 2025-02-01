using Microsoft.AspNetCore.Mvc;
using TBC.ROPP.Api.Abstractions;
using TBC.ROPP.Application.Services.Abstractions;

namespace TBC.ROPP.Api.Controllers;

[Route("api/file-storage")]
public class FileStorageController(IFileStorageService fileStorageService) : ApiControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetFileRecordAsync(Guid id, CancellationToken cancellation)
    {
        return Ok(await fileStorageService.GetFileRecordAsync(id, cancellation));
    }

    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> GetFileAsync(Guid id, CancellationToken cancellation)
    {
        var file = await fileStorageService.GetFileAsync(id, cancellation);

        return File(file.Stream, "application/octet-stream", $"{file.FileName}{file.Extension}");
    }

    [HttpPost]
    public async Task<IActionResult> UploadAsync(IFormFile file, CancellationToken cancellation)
    {
        return Ok(await fileStorageService.UploadAsync(file, cancellation));
    }
}