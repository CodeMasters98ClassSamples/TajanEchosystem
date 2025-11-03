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
}
