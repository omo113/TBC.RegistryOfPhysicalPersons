using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record UpdatePhysicalPersonCommand(int Id, UpdatePhysicalPersonModel Model) : IRequest<ApplicationResult<PhysicalPersonsDto, ApplicationError>>;

public class UpdatePhysicalPersonCommandValidator() : AbstractValidator<UpdatePhysicalPersonCommand>
{
    //todo
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