﻿using ChampionsChromo.Application.Albums.Commands.CreateAlbum;
using ChampionsChromo.Application.Albums.Commands.DeleteAlbum;
using ChampionsChromo.Application.Albums.Queries.GetAlbumById;
using ChampionsChromo.Application.Albums.Queries.GetAlbumBySchoolId;
using ChampionsChromo.Application.Albums.Queries.GetAlbums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    public async Task<IActionResult> Create(CreateAlbumCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetAlbumByIdQuery(id));

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    [HttpGet("schoolId/{schoolId}")]
    public async Task<IActionResult> GetBySchoolId(string schoolId)
    {
        var result = await _mediator.Send(new GetAlbumBySchoolIdQuery(schoolId));

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetAlbumsQuery());

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _mediator.Send(new DeleteAlbumCommand(id));

        if (result.IsSuccess)
            return Ok();

        return NotFound(result.Error);
    }
}
