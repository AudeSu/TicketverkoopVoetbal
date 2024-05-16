using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TicketverkoopVoetbal.Areas.Data;
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
        private readonly IAbonnementService<Abonnement> _abonnementService;
        private readonly IMatchService<Match> _matchService;
        private readonly ISeizoenService<Seizoen> _seizoenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public PurchaseAbonnementController(
            IService<Club> clubservice,
            IService<Zone> zoneService,
            IStoelService<Stoeltje> stoelservice,
            IAbonnementService<Abonnement> abonnementservice,
            IMatchService<Match> matchservice,
            ISeizoenService<Seizoen> seizoenservice,
            UserManager<ApplicationUser> userManager,
            IMapper mapper)
        {
            _clubService = clubservice;
            _zoneService = zoneService;
            _stoelService = stoelservice;
            _abonnementService = abonnementservice;
            _matchService = matchservice;
            _seizoenService = seizoenservice;
            _userManager = userManager;
            _mapper = mapper;
        }

        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            var club = await _clubService.FindById(Convert.ToInt32(id));
            var currentSeizoen = _seizoenService.GetNextSeizoen().Result;
            if (club == null)
            {
                return NotFound();
            }
            //if (CheckFullMatch(club.ClubId, currentSeizoen))
            //{
            //    return View("FullMatch");
            //}
            if (currentSeizoen == null)
            {
                return View("NoSeizoen");
            }
            if (CheckAbonnement(club.ClubId, currentSeizoen))
            {
                return View("HasAbonnement");
            }
            AbonnementVM abonnement = new()
            {
                ClubId = club.ClubId,
                StadionNaam = club.Thuisstadion.Naam,
                Naam = club.Naam,
                SeizoenId = currentSeizoen.SeizoenId,
                Seizoen = _mapper.Map<SeizoenVM>(currentSeizoen),
                Zones = new SelectList(await _zoneService.FilterById(club.Thuisstadion.StadionId), "ZoneId", "Naam")
            };
            return View(abonnement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
                if (VrijePlaatsen(abonnementVM))
                {
                    abonnementVM.Zones = new SelectList(await _zoneService.FilterById(club.Thuisstadion.StadionId), "ZoneId", "Naam", abonnementVM.ZoneId);
                    abonnementVM.Seizoen = _mapper.Map<SeizoenVM>(_seizoenService.GetNextSeizoen().Result);
                    TempData["ErrorVolzetMessage"] = $"Er zijn geen plaatsen meer beschikbaar in deze zone";
                    return View(abonnementVM);
                }
                CartAbonnementVM abonnement = _mapper.Map<CartAbonnementVM>(abonnementVM);
                {
                    abonnement.clubVM = _mapper.Map<ClubVM>(club);
                    abonnement.ZoneNaam = zone.Naam;
                    abonnement.Prijs = zone.PrijsAbonnement;
                    abonnement.DateCreated = DateTime.Now;
                };

                if (CheckShoppingCart(abonnement))
                {
                    return View("DoubleBooked");
                }
                ShoppingCartVM? shopping;

                if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
                {
                    shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
                }
                else
                {
                    shopping = new ShoppingCartVM();
                }
                if (shopping.Abonnementen == null)
                {
                    shopping.Abonnementen = new List<CartAbonnementVM>();
                }
                shopping.Abonnementen.Add(abonnement);

                HttpContext.Session.SetObject("ShoppingCart", shopping);

            }
            return RedirectToAction("Index", "ShoppingCart");
        }

        public Boolean CheckAbonnement(int? id, Seizoen currentSeizoen)
        {
            var hasAbonnement = false;
            var currentUserID = _userManager.GetUserId(User);
            var AbonnementList = _abonnementService.FindByStringId(currentUserID);

            foreach (var abonnement in AbonnementList.Result)
            {
                if (abonnement.ClubId == id && abonnement.SeizoenId == currentSeizoen.SeizoenId)
                {
                    hasAbonnement = true;
                }
            }

            return hasAbonnement;
        }

        public Boolean VrijePlaatsen(AbonnementVM abonnementVM)
        {
            Boolean isFull = false;
            var currentZone = _zoneService.FindById(Convert.ToInt32(abonnementVM.ZoneId)).Result;
            int aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(abonnementVM.ClubId, abonnementVM.ZoneId, abonnementVM.SeizoenId).Result.Count();
            var matchList = _matchService.FindByHomeClub(abonnementVM.ClubId).Result;
            foreach (var match in matchList)
            {
                if (match.SeizoenId == abonnementVM.SeizoenId)
                {
                    int aantalTicketPlaatsen = _stoelService.GetTakenSeatsByMatchID(match.MatchId, abonnementVM.ZoneId).Result.Count();
                    if (currentZone.Capaciteit - (aantalAbonnementPlaatsen + aantalTicketPlaatsen) <= 0)
                    {
                        isFull = true;
                        return isFull;
                    }
                }
            }

            return isFull;
        }

        //public Boolean CheckFullMatch(int id, Seizoen currentSeizoen)
        //{
        //    Boolean isFull = false;
        //    var matchList = _matchService.FindByHomeClub(id).Result;
        //    foreach (var match in matchList)
        //    {
        //        if (match.SeizoenId == currentSeizoen.SeizoenId)
        //        {
        //            int aantalVolleZones = 0;
        //            var zones = _zoneService.FilterById(match.Stadion.StadionId).Result;
        //            if (zones.Any())
        //            {
        //                foreach (var zone in zones)
        //                {

        //                    int aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(id, zone.ZoneId, currentSeizoen.SeizoenId).Result.Count();
        //                    int aantalTicketPlaatsen = _stoelService.GetTakenSeatsByMatchID(match.MatchId, zone.ZoneId).Result.Count();
        //                    var tiket = _stoelService.GetTakenSeatsByMatchID(match.MatchId, zone.ZoneId).Result;
        //                    if (zone.Capaciteit - (aantalAbonnementPlaatsen + aantalTicketPlaatsen) <= 0)
        //                    {
        //                        aantalVolleZones++;
        //                    }
        //                }


        //                if (aantalVolleZones == zones.Count())
        //                {
        //                    return isFull = true;
        //                }
        //            }


        //        }
        //    }
        //    return isFull;
        //}

        public Boolean CheckShoppingCart(CartAbonnementVM abonnement)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCart != null)
            {
                if (shoppingCart.Abonnementen != null && shoppingCart.Abonnementen.Any(a => a.ClubId == abonnement.ClubId))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
