using MediatR;
using Microsoft.AspNetCore.Mvc;
using Tajan.Standard.Domain.Wrappers;

namespace Tajan.OrderService.API.Controllers;

[Route("/api/[controller]/[action]")]
[ApiController]
public class BaseController : ControllerBase
{
    private ISender _mediator;
    private ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected async Task<ObjectResult> SendAsync<T>(IRequest<Result<T>> request, CancellationToken ct = default)
    {
        var result = await Mediator.Send(request, ct);

        if (result.IsSuccess)
            return Ok(result);

        return Ok(result);
    }

    protected async Task<Result<T>> SendDirectAsync<T>(IRequest<Result<T>> request, CancellationToken ct = default)
    {
        var result = await Mediator.Send(request, ct);
        return result;
    }

    protected async Task<ObjectResult> SendAsync(IRequest<Result<object>> request, CancellationToken ct = default)
        => await SendAsync<object>(request, ct);

    protected async Task<ObjectResult> SendAsync(IRequest<Result<Guid>> request, CancellationToken ct = default)
        => await SendAsync<Guid>(request, ct);

    protected async Task<ObjectResult> SendAsync(IRequest<Result<int>> request, CancellationToken ct = default)
        => await SendAsync<int>(request, ct);

    protected async Task<ObjectResult> SendAsync(IRequest<Result<long>> request, CancellationToken ct = default)
        => await SendAsync<long>(request, ct);
}

