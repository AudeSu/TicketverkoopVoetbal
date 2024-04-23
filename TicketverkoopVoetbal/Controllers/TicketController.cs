using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Diagnostics;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class TicketController : Controller
    {
        private IService<Match> _matchService;
        private IService<Zone> _zoneService;
        private readonly IMapper _mapper;

        public TicketController(IMapper mapper, IService<Match> matchservice, IService<Zone> zoneService)
        {
            _mapper = mapper;
            _matchService = matchservice;
            _zoneService = zoneService;
        }

        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var match = await _matchService.FindById(Convert.ToInt32(id));
            MatchVM matchVM = new MatchVM();
            matchVM = _mapper.Map<MatchVM>(match);
            if (match == null)
            {
                return NotFound();
            }
            TicketVM ticketVM = new TicketVM();
            ticketVM.MatchId = match.MatchId;
            ticketVM.matchVM = matchVM;
            ticketVM.Zones = new SelectList(
                await _zoneService.FilterById(match.StadionId), "ZoneId", "Naam");

            return View(ticketVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(TicketVM ticketVM)
        {
            if (ticketVM == null)
            {
                return NotFound();
            }
            try
            {
                var zone = await _zoneService.FindById(Convert.ToInt32(ticketVM.ZoneId));
                var match = await _matchService.FindById(Convert.ToInt32(ticketVM.MatchId));
                MatchVM matchVM = new MatchVM();
                matchVM = _mapper.Map<MatchVM>(match);
                ticketVM.matchVM = matchVM;
                ticketVM.Prijs = zone.Prijs;
                ticketVM.Zones =
                new SelectList(await _zoneService.FilterById(Convert.ToInt16(match.StadionId)), "ZoneId", "Naam", ticketVM.ZoneId);

                return View(ticketVM);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("errorlog" + ex.Message);
            }

            return View(ticketVM);
        }


        public async Task<IActionResult> Select(TicketVM ticketVM)
        {
            if (ticketVM == null)
            {
                return NotFound();
            }

            Match? match = await _matchService.FindById(Convert.ToInt32(ticketVM.MatchId));

            ShoppingCartVM? shopping;

            // var objComplex = HttpContext.Session.GetObject<ShoppingCartVM>("ComplexObject");
            if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
            {
                shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            }
            else
            {
                shopping = new ShoppingCartVM();
                shopping.Carts = new List<CartVM>();
            }

            if (match != null)
            {
                for (int i = 0; i < ticketVM.Aantal; i++)
                {
                    CartVM item = new CartVM
                    {
                        MatchId = ticketVM.MatchId,
                        matchVM = _mapper.Map<MatchVM>(match),
                        Aantal = ticketVM.Aantal,
                        Prijs = ticketVM.Prijs,
                        DateCreated = DateTime.Now

                    };
                    shopping?.Carts?.Add(item);
                }




                HttpContext.Session.SetObject("ShoppingCart", shopping);

            }
            return RedirectToAction("Index", "ShoppingCart");

        }
    }
}
