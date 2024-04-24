using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;
using TicketVerkoopVoetbal.Util.Mail.Interfaces;
using TicketVerkoopVoetbal.Util.PDF.Interfaces;

namespace TicketverkoopVoetbal.Controllers
{
    public class ShoppingCartController : Controller
    {
        private IEmailSend _emailsend;
        private ICreatePDF _createPDF;
        private IUserService<AspNetUser> _userService;

        public ShoppingCartController(
            IEmailSend emailsend, 
            ICreatePDF createPDF,
            IUserService<AspNetUser> userService)
        {
            _emailsend = emailsend;
            _createPDF = createPDF;
            _userService = userService;
        }

        public IActionResult Index()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            return View(cartList);
        }

        public IActionResult Delete(int? matchId)
        {
            if (matchId == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            CartVM? itemToRemove = cartList?.Carts?.FirstOrDefault(r => r.MatchId == matchId);

            if (itemToRemove != null)
            {
                cartList?.Carts?.Remove(itemToRemove);
                HttpContext.Session.SetObject("ShoppingCart", cartList);
            }
            return View("index", cartList);
        }

        //[Authorize]
        //[HttpPost]
        //public async Task<IActionResult> Payement()
        //{
        //    ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
        //    string? userID = User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    AspNetUser? user = await FindUser(userID);

        //    if (cartList == null
        //        || cartList.Cart == null
        //        || cartList.Cart.Count == 0
        //        || user == null)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    List<Zitplaat>
        //}

        private async Task<AspNetUser?> FindUser(string? userId)
        {
            if (userId != null)
            {
                return await _userService.FindByStringId(userId);
            }
            return null;
        }
    }
}
