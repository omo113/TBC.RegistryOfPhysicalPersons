using Microsoft.AspNetCore.Mvc;
using TBC.ROPP.Api.Abstractions;
using TBC.ROPP.Application.Services.Abstractions;

namespace TBC.ROPP.Api.Controllers;

[Route("api/file-storage")]
public class FileStorageController(IFileStorageService fileStorageService) : ApiControllerBase
{
    [HttpGet("{id:guid}/download")]
    public async Task<IActionResult> GetFileAsync(Guid id, CancellationToken cancellation)
    {
        var file = await fileStorageService.GetFileAsync(id, cancellation);

        return File(file.Stream, "application/octet-stream", $"{file.FileName}{file.Extension}");
    }
}