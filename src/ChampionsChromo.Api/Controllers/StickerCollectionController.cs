﻿using ChampionsChromo.Application.StickerCollection.Commands.CreateUserAlbum;
using ChampionsChromo.Application.StickerCollection.Commands.AddStickerToAlbum;
using ChampionsChromo.Application.StickerCollection.Commands.RemoveStickerFromAlbum;
using ChampionsChromo.Application.StickerCollection.Queries.GetStickerCollecionByAlbumIdAndUserId;
using ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbumById;
using ChampionsChromo.Application.StickerCollection.Queries.GetUserAlbums;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ChampionsChromo.Core.Enums;

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

    [HttpPatch("albumId/{albumId}/stickerNumber/{stickerNumber}/stickerType/{stickerType}/add")]
    public async Task<IActionResult> AddSticker(
        string albumId,
        int stickerNumber,
        StickerType stickerType)
    {
        var result = await _mediator.Send(new AddStickerToAlbumCommand(albumId, stickerNumber, stickerType));

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }

    [HttpPatch("albumId/{albumId}/stickerNumber/{stickerNumber}/stickerType/{stickerType}/remove")]
    public async Task<IActionResult> RemoveSticker(
        string albumId,
        int stickerNumber,
        StickerType stickerType)
    {
        var result = await _mediator.Send(new RemoveStickerFromAlbumCommand(albumId, stickerNumber, stickerType));

        if (result.IsSuccess)
            return Ok(result);

        return BadRequest(result.Error);
    }
}
