using Microsoft.AspNetCore.Mvc;
using Tajan.OrderService.Application.Usecases;
using Tajan.Standard.Presentation.Abstractions;

namespace Tajan.OrderService.API.Controllers.V1;

public class OrderController : CustomController
{
    [HttpPost]
    public async Task<IActionResult> Add([FromBody] AddOrderCommand command, CancellationToken ct = default)
        => await SendAsync(command, ct);

    [HttpPost]
    public async Task<IActionResult> CancelOrder([FromQuery] CancelOrderCommand command, CancellationToken ct = default)
        => await SendAsync(command, ct);
}
