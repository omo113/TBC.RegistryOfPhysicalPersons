using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Queries.PhysicalPersonQueries;

public class PhysicalPersonsQuery : IRequest<ApplicationResult<List<PhysicalPersonsDto>, ApplicationError>>
{
}

public class PhysicalPersonsQueryHandler(IRepository<PhysicalPerson> repository)
    : IRequestHandler<PhysicalPersonsQuery, ApplicationResult<List<PhysicalPersonsDto>, ApplicationError>>
{
    public async Task<ApplicationResult<List<PhysicalPersonsDto>, ApplicationError>> Handle(
        PhysicalPersonsQuery request, CancellationToken cancellationToken)
    {
        var query = await repository.Query()
            .Select(person => new PhysicalPersonsDto
            {
                Id = person.Id,
                Name = person.Name,
                LastName = person.LastName,
                City = person.City.Name,
                BirthDate = person.BirthDate,
                CreateDate = person.CreateDate,
                Gender = person.Gender,
                PersonalNumber = person.PersonalNumber,
                FileRecordUid = null
            })
            .ToListAsync(cancellationToken: cancellationToken);

        return new ApplicationResult<List<PhysicalPersonsDto>, ApplicationError>(query);
    }
}