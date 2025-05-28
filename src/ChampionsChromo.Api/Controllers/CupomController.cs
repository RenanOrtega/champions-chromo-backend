using ChampionsChromo.Application.Cupoms.Commands.CreateCupom;
using ChampionsChromo.Application.Cupoms.Commands.DeleteCupom;
using ChampionsChromo.Application.Cupoms.Commands.UpdateCupom;
using ChampionsChromo.Application.Cupoms.Queries.GetCupomById;
using ChampionsChromo.Application.Cupoms.Queries.GetCupoms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CupomController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateCupomCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetCupomByIdQuery(id));

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetCupomsQuery());

        return Ok(result.Value);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete([FromRoute] string id)
    {
        var result = await _mediator.Send(new DeleteCupomCommand(id));

        if (result.IsSuccess)
            return Ok();

        return NotFound(result.Error);
    }

    [HttpPatch("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(UpdateCupomCommand command, [FromRoute] string id)
    {
        command.Id = id;
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }
}
