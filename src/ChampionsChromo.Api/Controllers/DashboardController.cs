using ChampionsChromo.Application.Albums.Commands.CreateAlbum;
using ChampionsChromo.Application.Albums.Commands.DeleteAlbum;
using ChampionsChromo.Application.Albums.Commands.UpdateAlbum;
using ChampionsChromo.Application.Albums.Queries.GetAlbumById;
using ChampionsChromo.Application.Albums.Queries.GetAlbumBySchoolId;
using ChampionsChromo.Application.Albums.Queries.GetAlbums;
using ChampionsChromo.Application.Dashboard.Queries.GetMetrics;
using ChampionsChromo.Application.Schools.Commands.UpdateSchool;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet("metrics")]
    public async Task<IActionResult> GetMetrics()
    {
        var result = await _mediator.Send(new GetMetricsQuery());

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }
}
