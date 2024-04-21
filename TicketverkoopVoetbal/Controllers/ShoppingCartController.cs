using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class ShoppingCartController : Controller
    {
        public IActionResult Index()
        {

            ShoppingCartVM? cartList =
              HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            // call SessionID
            //var SessionId =  HttpContext.Session.Id;

            return View(cartList);
        }

        public IActionResult Delete(int? matchId)
        {
            if (matchId == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList
              = HttpContext.Session
              .GetObject<ShoppingCartVM>("ShoppingCart");

            CartVM? itemToRemove =
                cartList?.Carts?.FirstOrDefault(r => r.MatchId == matchId);
            // db.bieren.FirstOrDefault (r => 

            if (itemToRemove != null)
            {
                cartList?.Carts?.Remove(itemToRemove);
                HttpContext.Session.SetObject("ShoppingCart", cartList);

            }

            return View("index", cartList);

        }
    }
}
