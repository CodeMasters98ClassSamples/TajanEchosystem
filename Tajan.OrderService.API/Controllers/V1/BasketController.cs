using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tajan.OrderService.Application.Usecases.Order.Commands.AddBasketItem;
using Tajan.OrderService.Application.Usecases.Order.Commands.UpdateBasketItem;
using Tajan.OrderService.Application.Usecases.Order.Commands.RemoveBasketItem;
using Tajan.OrderService.Application.Usecases.Order.Commands.ClearBasket;

namespace Tajan.OrderService.API.Controllers.V1;

[ApiController]
[Route("api/[controller]")]
public class BasketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public BasketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> AddItem([FromBody] AddBasketItemCommand command)
    {
        await _mediator.Send(command);
        return Ok();
    }

    [HttpPut("{userId}/items/{productId}")]
    public async Task<IActionResult> UpdateItem([FromRoute] int userId, [FromRoute] int productId, [FromBody] UpdateBasketItemCommand command)
    {
        // ensure path and body match
        if (command.UserId != userId || command.ProductId != productId)
            return BadRequest("Path and body userId/productId must match");

        await _mediator.Send(command);
        return Ok();
    }

    [HttpDelete("{userId}/items/{productId}")]
    public async Task<IActionResult> RemoveItem([FromRoute] int userId, [FromRoute] int productId)
    {
        await _mediator.Send(new RemoveBasketItemCommand(userId, productId));
        return NoContent();
    }

    [HttpDelete("{userId}")]
    public async Task<IActionResult> ClearBasket([FromRoute] int userId)
    {
        await _mediator.Send(new ClearBasketCommand(userId));
        return NoContent();
    }

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket([FromRoute] int userId)
    {
        var result = await _mediator.Send(new Tajan.OrderService.Application.Usecases.Order.Queries.GetBasket.GetBasketQuery(userId));
        if (result == null) return NotFound();
        return Ok(result);
    }
}
