using Microsoft.AspNetCore.Mvc;
using Tajan.Identity.Application.AuthenticationUsecases;
using Tajan.Standard.Presentation.Abstractions;

namespace Tajan.Identity.API.Controllers;

public class AuthenticationController : CustomController
{
    //[HttpPost]
    //public async Task<IActionResult> Login(LoginCommand command, CancellationToken ct = default)
    //=> await SendAsync<object>(command, ct);

    [HttpGet]
    public async Task<IActionResult> Login(CancellationToken ct = default)
        => await SendAsync<object>(new LoginCommand("1", "1"), ct);
}
