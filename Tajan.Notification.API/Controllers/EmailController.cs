using Microsoft.AspNetCore.Mvc;

namespace Tajan.Notification.API.Controllers
{
    public class EmailController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
