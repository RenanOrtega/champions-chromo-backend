using ChampionsChromo.Application.StickerCollection.Commands.CreateUserAlbum;
using ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;
using ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbumById;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StickerCollectionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreateUserAlbumCommand command)
    {
        var result = await _mediator.Send(command);

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var result = await _mediator.Send(new GetUserAlbumByIdQuery(id));

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }

    //[HttpGet]
    //public async Task<IActionResult> GetAll()
    //{
    //    var result = await _mediator.Send(new GetUserAlbumsQuery());

    //    return Ok(result.Value);
    //}

    [HttpGet]
    public async Task<IActionResult> GetByUserId()
    {
        var result = await _mediator.Send(new GetStickerCollecionByUserIdQuery());

        return Ok(result.Value);
    }
}
