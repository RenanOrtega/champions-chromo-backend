using ChampionsChromo.Application.StickerCollection.Commands.CreateUserAlbum;
using ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;
using ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbumById;
using ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbums;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StickerCollectionController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost]
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _mediator.Send(new GetUserAlbumsQuery());

        return Ok(result.Value);
    }

    [HttpGet("userId/{userId}/albumId/{albumId}")]
    public async Task<IActionResult> GetByAlbumIdAndUserId(string userId, string albumId)
    {
        var result = await _mediator.Send(new GetStickerCollecionByAlbumIdAndUserIdQuery(albumId, userId));

        return Ok(result.Value);
    }
}
