using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using Tajan.Notification.API.Dtos;
using Tajan.Standard.Presentation.Abstractions;

namespace Tajan.Notification.API.Controllers;

public class SmsController : CustomController
{
    // /Notification/Sms/Single
    // /Notification/Sms/Bilk

    [HttpPost]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Single([FromBody] SmsDto sms)
    {
        //Send Notification Business
        return Ok();
    }
}
