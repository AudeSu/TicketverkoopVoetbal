using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.ViewModels;
using TicketVerkoopVoetbal.Util.Mail.Interfaces;

namespace TicketverkoopVoetbal.Controllers
{
    public class HomeController : Controller
    {
  
        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }

        //public IActionResult Contact()
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult Contact(SendMailVM sendMailVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {
        //            _emailSend.SendEmailAsync(sendMailVM.Email, "Contactformulier", sendMailVM.Message);
        //            return View("Thanks");
        //        }
        //        catch (Exception ex)
        //        {
        //            ModelState.AddModelError("", ex.Message);
        //        }
        //    }
        //    return View(sendMailVM);
        //}
    }
}
