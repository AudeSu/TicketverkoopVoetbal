using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;
using TicketVerkoopVoetbal.Util.Mail;
using TicketVerkoopVoetbal.Util.Mail.Interfaces;
using TicketVerkoopVoetbal.Util.PDF.Interfaces;

namespace TicketverkoopVoetbal.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IEmailSend _emailSend;
        private ICreatePDF _createPDF;
        private IUserService<AspNetUser> _userService;
        private readonly UserManager<IdentityUser> _userManager;


        public ShoppingCartController(
            IEmailSend emailsend, 
            ICreatePDF createPDF,
            IUserService<AspNetUser> userService,
            UserManager<IdentityUser> userManager)
        {
            _emailSend = emailsend;
            _createPDF = createPDF;
            _userService = userService;
            _userManager = userManager;
        }

        [Authorize]
        public IActionResult Index()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            return View(cartList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase()
        {
            var currentUser = await _userService.FindByStringId(GetCurrentUserId());
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            var message = "Bedankt om te bestellen op onze website u vind de tickets in de onderstaande bijlagen";
            if (cartList == null
                || cartList.Carts == null
                || cartList.Carts.Count == 0
                || currentUser == null)
            {
                return RedirectToAction("Index", "Match");
            }
            else
            {
          
                try
                {
                   
                    _emailSend.SendEmailAsync(currentUser.Email, "Contactformulier", message);
                    HttpContext.Session.Remove("ShoppingCart");
                    return View("Thanks");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", ex.Message);
                }
            }
            return View();
        }

        public IActionResult DeleteTicket(int? matchId)
        {
            if (matchId == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            CartTicketVM? itemToRemove = cartList?.Carts?.FirstOrDefault(r => r.MatchId == matchId);

            if (itemToRemove != null)
            {
                cartList?.Carts?.Remove(itemToRemove);
                HttpContext.Session.SetObject("ShoppingCart", cartList);
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAbonnement()
        {

            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            CartAbonnementVM? itemToRemove = cartList?.Abonnement;

            if (itemToRemove != null)
            {
                cartList.Abonnement = null;
                HttpContext.Session.SetObject("ShoppingCart", cartList);
            }
            return RedirectToAction("Index");
        }

        private string GetCurrentUserId()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return "fout";
            }
        }


    }
}
