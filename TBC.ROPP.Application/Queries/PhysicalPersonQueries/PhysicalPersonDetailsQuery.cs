using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Queries.PhysicalPersonQueries;

public record PhysicalPersonDetailsQuery(int Id) : IRequest<ApplicationResult<PhysicalPersonDetailsDto, ApplicationError>>;

public class PhysicalPersonDetailsQueryValidator : AbstractValidator<PhysicalPersonDetailsQuery>
{
    public PhysicalPersonDetailsQueryValidator()
    {

    }
}
public class PhysicalPersonDetailsQueryHandler(IRepository<PhysicalPerson> repository) : IRequestHandler<PhysicalPersonDetailsQuery, ApplicationResult<PhysicalPersonDetailsDto, ApplicationError>>
{
    public async Task<ApplicationResult<PhysicalPersonDetailsDto, ApplicationError>> Handle(PhysicalPersonDetailsQuery request, CancellationToken cancellationToken)
    {
        var person = await repository.Query(x => x.Id == request.Id)
            .Include(x => x.City)
            .Include(x => x.RelatedPeopleList)
            .ThenInclude(x => x.RelatedPhysicalPerson)
            .Include(x => x.FileRecord)
            .Select(x => new PhysicalPersonDetailsDto
            {
                BirthDate = x.BirthDate,
                City = x.City.Name,
                CityId = x.CityId,
                CreateDate = x.CreateDate,
                Gender = x.Gender,
                Id = x.Id,
                LastChangeDate = x.LastChangeDate,
                PhoneNumbers = x.PhoneNumbers.Select(phoneNumber =>
                    new PhoneNumberDto(phoneNumber.PhoneNumberType, phoneNumber.Number)),
                PersonalNumber = x.PersonalNumber,
                LastName = x.LastName,
                FileUid = x.FileRecord != null ? x.FileRecord.UId : null,
                Name = x.Name,
                RelatedPerson = x.RelatedPeopleList.Select(related => new RelatedPersonDto(
                    related.PersonRelationshipType, related.RelatedPhysicalPersonId,
                    related.RelatedPhysicalPerson.Name, related.RelatedPhysicalPerson.LastName))

            })
            .FirstAsync(cancellationToken: cancellationToken);
        return new ApplicationResult<PhysicalPersonDetailsDto, ApplicationError>(person);
    }
}