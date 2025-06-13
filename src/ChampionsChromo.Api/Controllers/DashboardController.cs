using ChampionsChromo.Application.Dashboard.Queries.GetMetrics;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DashboardController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;


    [HttpGet("metrics")]
    [Authorize]
    public async Task<IActionResult> GetMetrics([FromQuery] int daysBack = 30)
    {
        var result = await _mediator.Send(new GetMetricsQuery { DaysBack = daysBack });

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }
}
