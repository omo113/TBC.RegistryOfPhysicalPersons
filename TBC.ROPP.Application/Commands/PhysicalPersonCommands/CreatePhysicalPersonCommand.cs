using FluentValidation;
using MediatR;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.ValueObjects;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record CreatePhysicalPersonCommand(CreatePhysicalPersonDto Model) : IRequest<ApplicationResult<PhysicalPersonsDto, ApplicationError>>;

public class CreatePhysicalPersonCommandValidator() : AbstractValidator<CreatePhysicalPersonCommand>
{
    //todo
}

public class CreatePhysicalPersonCommandHandler(IRepository<PhysicalPerson> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePhysicalPersonCommand, ApplicationResult<PhysicalPersonsDto, ApplicationError>>
{
    public async Task<ApplicationResult<PhysicalPersonsDto, ApplicationError>> Handle(CreatePhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = request.Model.PhoneNumbers.Select(x => PhoneNumber.Create(x.NumberType, x.TelephoneNumber));
        var physicalPerson = PhysicalPersonHelpers.CreatePhysicalPerson(request.Model, phoneNumber);
        await repository.Store(physicalPerson);
        await unitOfWork.SaveAsync(cancellationToken);
        return new ApplicationResult<PhysicalPersonsDto, ApplicationError>(physicalPerson.CreatePhysicalPersonDto());
    }

}