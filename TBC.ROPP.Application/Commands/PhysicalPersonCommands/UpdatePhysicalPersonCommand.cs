using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Commands.PhysicalPersonCommands.Helpers;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;
using TBC.ROPP.Shared.Translation;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record UpdatePhysicalPersonCommand(int Id, UpdatePhysicalPersonModel Model) : IRequest<ApplicationResult<PhysicalPersonsDto, ApplicationError>>;

public class UpdatePhysicalPersonCommandValidator : AbstractValidator<UpdatePhysicalPersonCommand>
{
    public UpdatePhysicalPersonCommandValidator(IRepository<City> cityRepository, IRepository<PhysicalPerson> repository)
    {
        RuleFor(x => x.Id)
            .NotEmpty()
            .MustAsync(async (person, token) =>
            {
                return await repository.Query(x => x.Id == person).AnyAsync(token);
            })
            .WithMessage(Translation.Translate("PhysicalPersonNotExist"))
            .DependentRules(() =>
            {
                RuleFor(x => x.Model)
                    .NotNull();
                RuleFor(x => x.Model.Name)
                    .NotEmpty()
                    .Length(2, 50)
                    .Must(BeValidName)
                    .WithMessage(Translation.Translate("NameMustBeGeorgianOrLatin"));


                RuleFor(x => x.Model.LastName)
                    .NotEmpty()
                    .Length(2, 50)
                    .Must(BeValidName)
                    .WithMessage(Translation.Translate("LastNameMustBeGeorgianOrLatin"));


                RuleFor(x => x.Model.Gender)
                    .IsInEnum();


                RuleFor(x => x.Model.BirthDate)
                    .Must(BeAtLeast18YearsOld)
                    .WithMessage(Translation.Translate("MustBeOver18"));

                RuleFor(x => x.Model.CityId)
                    .NotEmpty()
                    .MustAsync(async (city, token) =>
                    {
                        return await cityRepository.Query(x => x.Id == city).AnyAsync(token);
                    })
                    .WithMessage(Translation.Translate("CityNotExist"));

                RuleForEach(x => x.Model.PhoneNumbers)
                    .ChildRules(phone =>
                    {
                        phone.RuleFor(p => p.NumberType)
                            .IsInEnum();

                        phone.RuleFor(p => p.TelephoneNumber)
                            .NotEmpty()
                            .Length(4, 50);
                    });
            });

    }
    private bool BeAtLeast18YearsOld(DateTimeOffset birthDate)
    {
        var today = DateTimeOffset.Now;
        var age = today.Year - birthDate.Year;
        if (birthDate > today.AddYears(-age))
            age--;

        return age >= 18;
    }
    private bool BeValidName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            return false;

        bool isLatin = RegexHelpers.GeneratedLatinRegex().IsMatch(name);
        bool isGeorgian = RegexHelpers.GeneratedGeorgianRegex().IsMatch(name);

        return isLatin ^ isGeorgian;
    }
}

public class UpdatePhysicalPersonCommandHandler(IRepository<PhysicalPerson> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<UpdatePhysicalPersonCommand, ApplicationResult<PhysicalPersonsDto, ApplicationError>>
{
    public async Task<ApplicationResult<PhysicalPersonsDto, ApplicationError>> Handle(UpdatePhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = await repository.Query(x => x.Id == request.Id)
                                                       .Include(x => x.PhoneNumbers)
                                                       .FirstAsync(cancellationToken: cancellationToken);
        return await phoneNumber
            .UpdateFields(request.Model.Name,
                request.Model.LastName,
                request.Model.Gender,
                request.Model.BirthDate,
                request.Model.CityId,
                request.Model.PhoneNumbers.Select(x => PhoneNumber.Create(x.NumberType, x.TelephoneNumber)).ToList())
            .MatchAsync(async (res) =>
                {
                    await unitOfWork.SaveAsync(cancellationToken);
                    return new ApplicationResult<PhysicalPersonsDto, ApplicationError>(res.CreatePhysicalPersonDto());
                },
                validation => validation.ToApplicationResultAsync<PhysicalPersonsDto>());
    }

}