using ChampionsChromo.Application.Schools.Commands.CreateSchool;
using ChampionsChromo.Application.Schools.Queries.GetSchoolById;
using ChampionsChromo.Application.Schools.Queries.GetSchools;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateSchoolCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok();

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetSchoolByIdQuery(id));

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetSchoolsQuery());

        return Ok(result.Value);
    }
}
