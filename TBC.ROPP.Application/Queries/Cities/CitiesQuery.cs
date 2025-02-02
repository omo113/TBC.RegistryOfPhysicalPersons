using MediatR;
using Microsoft.EntityFrameworkCore;
using TBC.ROPP.Application.Shared;
using TBC.ROPP.Domain.Entities;
using TBC.ROPP.Infrastructure.Repositories.Abstractions;
using TBC.ROPP.Shared.ApplicationInfrastructure;

namespace TBC.ROPP.Application.Queries.Cities;

public record CityDto(int Id, string Name);
public record CitiesQuery() : IRequest<ApplicationResult<List<CityDto>, ApplicationError>>;


public class CitiesQueryHandler(IRepository<City> repository)
    : IRequestHandler<CitiesQuery, ApplicationResult<List<CityDto>, ApplicationError>>
{
    public async Task<ApplicationResult<List<CityDto>, ApplicationError>> Handle(CitiesQuery request, CancellationToken cancellationToken)
    {
        var data = await repository.Query()
            .Select(x => new CityDto(x.Id, CultureUtils.GetCurrentCultureIsGeorgian() ? x.NameGe : x.Name))
            .ToListAsync(cancellationToken: cancellationToken);
        return new ApplicationResult<List<CityDto>, ApplicationError>(data);
    }
}