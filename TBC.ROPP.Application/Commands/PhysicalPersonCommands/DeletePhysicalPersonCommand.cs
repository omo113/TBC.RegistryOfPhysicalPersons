using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record DeletePhysicalPersonCommand(int Id) : IRequest<ApplicationResult<bool, ApplicationError>>;

public class DeletePhysicalPersonCommandValidator : AbstractValidator<DeletePhysicalPersonCommand>
{

}
public class DeletePhysicalPersonCommandHandler(IRepository<PhysicalPerson> repository, IUnitOfWork unitOfWork) : IRequestHandler<DeletePhysicalPersonCommand, ApplicationResult<bool, ApplicationError>>
{
    public async Task<ApplicationResult<bool, ApplicationError>> Handle(DeletePhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var person = await repository.Query(x => x.Id == request.Id).FirstAsync(cancellationToken: cancellationToken);
        person.Delete();
        await unitOfWork.SaveAsync(cancellationToken);
        return new ApplicationResult<bool, ApplicationError>(true);
    }
}