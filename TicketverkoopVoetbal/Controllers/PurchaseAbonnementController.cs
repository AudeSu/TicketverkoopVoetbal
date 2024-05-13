using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class PurchaseAbonnementController : Controller
    {
        private readonly IService<Club> _clubService;
        private readonly IService<Zone> _zoneService;
        private readonly IStoelService<Stoeltje> _stoelService;
        private readonly IMapper _mapper;

        public PurchaseAbonnementController(
            IService<Club> clubservice,
            IService<Zone> zoneService,
            IStoelService<Stoeltje> stoelservice,
            IMapper mapper)
        {
            _clubService = clubservice;
            _zoneService = zoneService;
            _stoelService = stoelservice;
            _mapper = mapper;
        }
        public async Task<IActionResult> Index(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var club = await _clubService.FindById(Convert.ToInt32(id));
            AbonnementVM abonnement = new AbonnementVM();
            abonnement.ClubId = club.ClubId;
            abonnement.StadionNaam = club.Thuisstadion.Naam;
            abonnement.Naam = club.Naam;
            abonnement.Zones = new SelectList(await _zoneService.FilterById(club.Thuisstadion.StadionId), "ZoneId", "Naam");
            return View(abonnement);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AbonnementVM abonnementVM)
        {
            if (abonnementVM == null)
            {
                return NotFound();
            }

            var club = await _clubService.FindById(Convert.ToInt32(abonnementVM.ClubId));
            var zone = await _zoneService.FindById(Convert.ToInt32(abonnementVM.ZoneId));
            if (club != null)
            {
                CartAbonnementVM abonnement = new CartAbonnementVM
                {
                    ClubId = abonnementVM.ClubId,
                    GebruikerID = abonnementVM.GebruikerID,
                    StoeltjeId = abonnementVM.StoeltjeId,
                    ZoneId = abonnementVM.ZoneId,
                    clubVM = _mapper.Map<ClubVM>(club),
                    ZoneNaam = zone.Naam,
                    Prijs = zone.Prijs,
                    DateCreated = DateTime.Now
                };

                ShoppingCartVM? shopping;

                // var objComplex = HttpContext.Session.GetObject<ShoppingCartVM>("ComplexObject");
                if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
                {
                    shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
                }
                else
                {
                    shopping = new ShoppingCartVM();
                }
                shopping.Abonnement = abonnement;

                HttpContext.Session.SetObject("ShoppingCart", shopping);
            }
            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
