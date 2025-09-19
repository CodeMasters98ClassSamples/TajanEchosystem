using Microsoft.AspNetCore.Mvc;

namespace Tajan.Notification.API.Controllers
{
    public class PushController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
