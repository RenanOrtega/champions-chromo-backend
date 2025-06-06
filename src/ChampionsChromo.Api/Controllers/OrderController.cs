using ChampionsChromo.Application.Orders.Commands.CreateOrder;
using ChampionsChromo.Application.Orders.Commands.DeleteOrder;
using ChampionsChromo.Application.Orders.Commands.UpdateOrder;
using ChampionsChromo.Application.Orders.Queries.GetOrderById;
using ChampionsChromo.Application.Orders.Queries.GetOrders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChampionsChromo.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost]
        public async Task<IActionResult> Create(CreateOrderCommand command)
        {
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Error);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var result = await _mediator.Send(new GetOrderByIdQuery(id));

            if (result.IsSuccess)
                return Ok(result.Value);

            return NotFound(result.Error);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetOrdersQuery());

            return Ok(result.Value);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            var result = await _mediator.Send(new DeleteOrderCommand(id));

            if (result.IsSuccess)
                return Ok();

            return NotFound(result.Error);
        }

        [HttpPatch("{id}")]
        [Authorize]
        public async Task<IActionResult> Update(UpdateOrderCommand command, [FromRoute] string id)
        {
            command.Id = id;
            var result = await _mediator.Send(command);

            if (result.IsSuccess)
                return Ok(result);

            return BadRequest(result.Error);
        }
    }
}
