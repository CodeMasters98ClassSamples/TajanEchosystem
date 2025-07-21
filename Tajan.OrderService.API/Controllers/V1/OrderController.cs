using Microsoft.AspNetCore.Mvc;
using Tajan.OrderService.Application.Usecases;

namespace Tajan.OrderService.API.Controllers.V1;

//[ApiVersion(1)]
public class OrderController : BaseController
{
    //[MapToApiVersion(1)]
    [HttpPost]
    public async Task<IActionResult> Add([FromQuery] AddOrderCommand command, CancellationToken ct = default)
        => await SendAsync(command, ct);
}
