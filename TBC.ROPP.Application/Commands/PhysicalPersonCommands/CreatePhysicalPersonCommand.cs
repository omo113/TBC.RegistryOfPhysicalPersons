using FluentValidation;
using MediatR;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record CreatePhysicalPersonCommand(PhysicalPersonDto Model) : IRequest<ApplicationResult<bool, ApplicationError>>;

public class CreatePhysicalPersonCommandValidator() : AbstractValidator<CreatePhysicalPersonCommand>
{
    //todo
}

public class CreatePhysicalPersonCommandHandler(IRepository<PhysicalPerson> repository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreatePhysicalPersonCommand, ApplicationResult<bool, ApplicationError>>
{
    public async Task<ApplicationResult<bool, ApplicationError>> Handle(CreatePhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var phoneNumber = PhoneNumber.Create(request.Model.NumberType, request.Model.PhoneNumber);
        var physicalPerson = PhysicalPersonHelpers.CreatePhysicalPerson(request.Model, phoneNumber);
        await repository.Store(physicalPerson);
        await unitOfWork.SaveAsync(cancellationToken);
        return new ApplicationResult<bool, ApplicationError>(true);
    }

}