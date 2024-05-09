using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class PurchaseAbonnementController : Controller
    {
        private IService<Club> _clubService;
        private IService<Zone> _zoneService;
        private IStoelService<Stoeltje> _stoelService;
        private readonly IMapper _mapper;

        public PurchaseAbonnementController(
            IMapper mapper, 
            IService<Club> clubservice, 
            IService<Zone> zoneService,
            IStoelService<Stoeltje> stoelservice
            )
        {
            _mapper = mapper;
            _clubService = clubservice;
            _zoneService = zoneService;
            _stoelService = stoelservice;   
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
            abonnement.Zones = new SelectList(
                await _zoneService.FilterById(club.Thuisstadion.StadionId), "ZoneId", "Naam");

            return View(abonnement);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AbonnementVM abonnementVM)
        {
            if (abonnementVM == null)
            {
                return NotFound();
            }
            try
            {
                var zone = await _zoneService.FindById(Convert.ToInt32(abonnementVM.ZoneId));
                var club = await _clubService.FindById(Convert.ToInt32(abonnementVM.ClubId));
                abonnementVM.Prijs = zone.Prijs;
                abonnementVM.Zones =
                new SelectList(await _zoneService.FilterById(Convert.ToInt16(club.ThuisstadionId)), "ZoneId", "Naam", abonnementVM.ZoneId);
                HttpContext.Session.SetObject("AbonnementVM", abonnementVM);

                abonnementVM.AantalVrijePlaatsen = VrijePlaatsen(abonnementVM);
                return View(abonnementVM);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("errorlog" + ex.Message);
            }

            return View(abonnementVM);
        }

        public int VrijePlaatsen(AbonnementVM abonnementVM)
        {
            var currentZone = _zoneService.FindById(Convert.ToInt32(abonnementVM.ZoneId)).Result;

            int aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(abonnementVM.matchVM.ClubId, abonnementVM.ZoneId, abonnementVM.matchVM.SeizoenID).Result.Count();
            int aantalticketPlaatsen = _stoelService.GetTakenSeatsByMatchID(abonnementVM.MatchId, abonnementVM.ZoneId).Result.Count();

            return currentZone.Capaciteit - (aantalAbonnementPlaatsen + aantalticketPlaatsen);
        }

        public async Task<IActionResult> Select()
        {

            var abonnementVM = HttpContext.Session.GetObject<AbonnementVM>("AbonnementVM");
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
                    Prijs = abonnementVM.Prijs,
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
