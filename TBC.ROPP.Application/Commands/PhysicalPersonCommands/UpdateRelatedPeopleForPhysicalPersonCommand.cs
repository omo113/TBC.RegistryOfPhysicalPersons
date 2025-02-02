using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate.Entities;
using TBC.ROPP.Domain.Shared;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Infrastructure.UnitOfWork.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Commands.PhysicalPersonCommands;

public record UpdateRelatedPeopleForPhysicalPersonCommand(int Id, UpdateRelatedPersonDto[] RelatedPersonModel) : IRequest<ApplicationResult<PhysicalPersonsDto, ApplicationError>>;

public class UpdateRelatedPeopleForPhysicalPersonCommandValidator : AbstractValidator<
    UpdateRelatedPeopleForPhysicalPersonCommand>
{

}

public class UpdateRelatedPeopleForPhysicalPersonCommandHandler(IRepository<PhysicalPerson> personRepository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateRelatedPeopleForPhysicalPersonCommand, ApplicationResult<PhysicalPersonsDto, ApplicationError>>
{
    public async Task<ApplicationResult<PhysicalPersonsDto, ApplicationError>> Handle(UpdateRelatedPeopleForPhysicalPersonCommand request, CancellationToken cancellationToken)
    {
        var person = await personRepository.Query(x => x.Id == request.Id)
                                           .Include(x => x.RelatedPeopleList)
                                           .FirstAsync(cancellationToken: cancellationToken);

        return await person.UpdateRelatedPeople(request.RelatedPersonModel
                .Select(x => RelatedPerson.Create(x.RelationshipType, person.Id, x.PersonId)).ToList())
            .MatchAsync(async (res) =>
                {
                    await unitOfWork.SaveAsync(cancellationToken);
                    return new ApplicationResult<PhysicalPersonsDto, ApplicationError>(res.CreatePhysicalPersonDto());
                },
                validation => validation.ToApplicationResultAsync<PhysicalPersonsDto>());

    }
}