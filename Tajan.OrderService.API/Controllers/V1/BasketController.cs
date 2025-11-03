using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tajan.OrderService.Application.Usecases.Order.Commands.AddBasketItem;

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

    [HttpGet("{userId}")]
    public async Task<IActionResult> GetBasket([FromRoute] int userId)
    {
        var result = await _mediator.Send(new Tajan.OrderService.Application.Usecases.Order.Queries.GetBasket.GetBasketQuery(userId));
        if (result == null) return NotFound();
        return Ok(result);
    }
}
