using MediatR;
using Microsoft.AspNetCore.Mvc;
using TBC.ROPP.Application.Queries.Cities;

namespace TBC.ROPP.Api.Controllers;

[Route("api/city")]
public class CityController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAsync(CitiesQuery query, CancellationToken cancellation)
        => (await mediator.Send(query, cancellation)).Match(Ok, IActionResult (error) => BadRequest(error));
}