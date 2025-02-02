using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record DeletePhysicalPersonCommand(int Id) : IRequest<ApplicationResult<PhysicalPersonsDto, ApplicationError>>;

public class DeletePhysicalPersonCommandValidator : AbstractValidator<DeletePhysicalPersonCommand>
{

}
public class DeletePhysicalPersonCommandHandler(IRepository<PhysicalPerson> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeletePhysicalPersonCommand, ApplicationResult<PhysicalPersonsDto, ApplicationError>>
{
    public async Task<ApplicationResult<PhysicalPersonsDto, ApplicationError>> Handle(DeletePhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.Query(x => x.Id == request.Id).FirstAsync(cancellationToken: cancellationToken);
        person.Delete();
        await unitOfWork.SaveAsync(cancellationToken);
        return new ApplicationResult<PhysicalPersonsDto, ApplicationError>(person.CreatePhysicalPersonDto());
    }
}