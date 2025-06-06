﻿using ChampionsChromo.Application.Schools.Commands.CreateSchool;
using ChampionsChromo.Application.Schools.Commands.DeleteSchool;
using ChampionsChromo.Application.Schools.Commands.UpdateSchool;
using ChampionsChromo.Application.Schools.Queries.GetSchoolById;
using ChampionsChromo.Application.Schools.Queries.GetSchools;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SchoolController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Authorize]
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

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _mediator.Send(new DeleteSchoolCommand(id));

        if (result.IsSuccess)
            return Ok();

        return NotFound(result.Error);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(UpdateSchoolCommand command, [FromRoute] string id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }
}
