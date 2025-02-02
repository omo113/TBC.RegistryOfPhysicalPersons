using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Queries.PhysicalPersonQueries;



//todo ეს შეიძლებოდა ცალკე ცხრილში გვქონოდა სადაც message bus ით events დავამუშავებდით და ყოველ ჯერზე დავითვილით
public record PhysicalPersonReportQuery() : IRequest<ApplicationResult<object, ApplicationError>>;


public class PhysicalPersonReportQueryHandler(IRepository<PhysicalPerson> repository) : IRequestHandler<PhysicalPersonReportQuery, ApplicationResult<object, ApplicationError>>
{
    public async Task<ApplicationResult<object, ApplicationError>> Handle(PhysicalPersonReportQuery request, CancellationToken cancellationToken)
    {



        return new ApplicationResult<object, ApplicationError>(await repository.Query()
            .Select(person => new PhysicalPersonReport
            {
                Name = person.Name,
                LastName = person.LastName,
                PersonalNumber = person.PersonalNumber,
                RelatedPeopleCounts = person.RelatedPeopleList
                    .GroupBy(rp => rp.PersonRelationshipType)
                    .Select(g => new RelatedPeopleCount
                    {
                        RelationshipType = g.Key,
                        Count = g.Count()
                    }).ToList()
            })
            .ToListAsync(cancellationToken: cancellationToken));
    }
}