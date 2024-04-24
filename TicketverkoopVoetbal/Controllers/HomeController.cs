using Microsoft.AspNetCore.Mvc;

namespace TicketverkoopVoetbal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
