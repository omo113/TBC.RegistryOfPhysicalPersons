using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Helpers;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Application.Shared;
using TBC.ROPP.Domain.Aggregates.PhysicalPersonAggregate;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Queries.PhysicalPersonQueries;

public class PhysicalPersonsQuery : Query, IRequest<ApplicationResult<QueryResult<PhysicalPersonsDto>, ApplicationError>>
{
    public string? Name { get; set; }
    public string? LastName { get; set; }
    public string? PersonalNumber { get; set; }
}

public class PhysicalPersonsQueryHandler(IRepository<PhysicalPerson> repository)
    : IRequestHandler<PhysicalPersonsQuery, ApplicationResult<QueryResult<PhysicalPersonsDto>, ApplicationError>>
{
    public async Task<ApplicationResult<QueryResult<PhysicalPersonsDto>, ApplicationError>> Handle(
        PhysicalPersonsQuery request, CancellationToken cancellationToken)
    {
        var skip = request.PageIndex * request.PageSize;
        var take = request.PageSize;


        var query = repository.Query()
            .WhereIf(!string.IsNullOrWhiteSpace(request.Name), p =>
                p.Name.Contains(request.Name!.Trim()))
            .WhereIf(!string.IsNullOrWhiteSpace(request.LastName), p =>
                p.LastName.Contains(request.LastName!.Trim()))
            .WhereIf(!string.IsNullOrWhiteSpace(request.PersonalNumber), p =>
                p.PersonalNumber.Contains(request.PersonalNumber!.Trim()))
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
            });
        var data = await query
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);

        return new ApplicationResult<QueryResult<PhysicalPersonsDto>, ApplicationError>(new QueryResult<PhysicalPersonsDto>
        {
            Result = data,
            TotalSize = await query.CountAsync(cancellationToken: cancellationToken)
        });
    }
}