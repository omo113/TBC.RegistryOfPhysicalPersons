using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Application.Services.Abstractions;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Persistance;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;
using TBC.ROPP.Shared.Translation;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record UploadPhysicalPersonImageCommand(int Id, IFormFile File) : IRequest<ApplicationResult<PhysicalPersonsDto, ApplicationError>>;

public class UploadPhysicalPersonImageCommandValidator : AbstractValidator<UploadPhysicalPersonImageCommand>
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".svg", ".webp"];

    public UploadPhysicalPersonImageCommandValidator(IRepository<PhysicalPerson> repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (person, token) => { return await repository.Query(x => x.Id == person).AnyAsync(token); })
            .WithMessage(Translation.Translate("PhysicalPersonNotExist"))
            .DependentRules(() =>
            {
                RuleFor(x => x.File)
                    .Must(x =>
                    {
                        string fileExtension = Path.GetExtension(x.FileName).ToLowerInvariant();
                        return AllowedExtensions.Contains(fileExtension);
                    })
                    .WithMessage(Translation.Translate("FileMustBeAnImage"));
            });
    }
}
public class UploadPhysicalPersonImageCommandHandler(ApplicationDbContext dbContext, IRepository<PhysicalPerson> physicalPersonRepository,
                                                     IRepository<FileRecord> fileRecordRepository, IFileStorageService fileStorageService, IUnitOfWork unitOfWork)
                                                            : IRequestHandler<UploadPhysicalPersonImageCommand, ApplicationResult<PhysicalPersonsDto, ApplicationError>>
{
    public async Task<ApplicationResult<PhysicalPersonsDto, ApplicationError>> Handle(UploadPhysicalPersonImageCommand request, CancellationToken cancellationToken)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            var uploadFileUid = await fileStorageService.UploadAsync(request.File, cancellationToken);
            var file = await fileRecordRepository.Query(x => x.UId == uploadFileUid.Id).FirstAsync(cancellationToken: cancellationToken);
            var person = await physicalPersonRepository.Query(x => x.Id == request.Id)
                                                       .Include(x => x.FileRecord)
                                                       .FirstAsync(cancellationToken: cancellationToken);
            return await person.UpdateFile(file.Id)
                .MatchAsync(async (res) =>
                {
                    await unitOfWork.SaveAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);
                    return new ApplicationResult<PhysicalPersonsDto, ApplicationError>(res.CreatePhysicalPersonDto());
                },
                    validation => validation.ToApplicationResultAsync<PhysicalPersonsDto>());
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
}
