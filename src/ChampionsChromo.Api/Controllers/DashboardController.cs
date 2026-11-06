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
    [Authorize(Roles = "admin")]
    public async Task<IActionResult> GetMetrics(
        [FromQuery] DateTimeOffset? from = null,
        [FromQuery] DateTimeOffset? to = null,
        [FromQuery] int? daysBack = null)
    {
        // Sem parâmetros = período todo.
        // from é inclusivo e to é exclusivo; ambos devem ser enviados com fuso (ex.: ISO 8601 com "Z").
        // daysBack é mantido por compatibilidade e só vale quando from não é informado.
        var fromUtc = from?.UtcDateTime;
        var toUtc = to?.UtcDateTime;

        if (fromUtc is null && daysBack is > 0)
            fromUtc = DateTime.UtcNow.AddDays(-daysBack.Value);

        if (fromUtc is not null && toUtc is not null && fromUtc >= toUtc)
            return BadRequest("A data inicial deve ser anterior à data final.");

        var result = await _mediator.Send(new GetMetricsQuery { From = fromUtc, To = toUtc });

        if (result.IsSuccess)
            return Ok(result.Value);

        return NotFound(result.Error);
    }
}
