using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace TicketverkoopVoetbal.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult SetAppLanguage(string lang, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(lang)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1)
                });

            return LocalRedirect(returnUrl);
        }
    }
}
