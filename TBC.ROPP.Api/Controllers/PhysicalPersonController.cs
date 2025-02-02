using MediatR;
using Microsoft.AspNetCore.Mvc;
using TBC.ROPP.Api.Abstractions;
using TBC.ROPP.Application.Commands.PhysicalPersonCommands;
using TBC.ROPP.Application.Models.Person;
using TBC.ROPP.Application.Queries.PhysicalPersonQueries;

namespace TBC.ROPP.Api.Controllers;

[Route("api/v1/physical-person")]
public class PhysicalPersonController(IMediator mediator) : ApiControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreatePhysicalPersonModel model)
    {
        return (await mediator.Send(new CreatePhysicalPersonCommand(model))).Match<IActionResult>(Ok, BadRequest);
    }
    [HttpPut("update/{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdatePhysicalPersonModel model)
    {
        return (await mediator.Send(new UpdatePhysicalPersonCommand(id, model))).Match<IActionResult>(Ok, BadRequest);
    }
    [HttpDelete("delete/{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        return (await mediator.Send(new DeletePhysicalPersonCommand(id))).Match<IActionResult>(Ok, BadRequest);
    }
    [HttpPost("update-related-people")]
    public async Task<IActionResult> UpdateRelatedPeopleAsync([FromBody] UpdateRelatedPeopleForPhysicalPersonCommand command)
    {
        return (await mediator.Send(command)).Match<IActionResult>(Ok, BadRequest);
    }
    [HttpPost("update-image")]
    public async Task<IActionResult> UpdateRelatedPeopleAsync([FromBody] UploadPhysicalPersonImageCommand command)
    {
        return (await mediator.Send(command)).Match<IActionResult>(Ok, BadRequest);
    }
    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetDetailsAsync(int id)
    {
        return (await mediator.Send(new PhysicalPersonDetailsQuery(id))).Match(Ok, error => BadRequest(error) as IActionResult);
    }
    [HttpGet("report")]
    public async Task<IActionResult> GetReport()
    {
        return (await mediator.Send(new PhysicalPersonReportQuery())).Match(Ok, error => BadRequest(error) as IActionResult);
    }
}