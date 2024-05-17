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
            if (!id.HasValue)
            {
                return NotFound();
            }

            var club = await _clubService.FindById(id.Value);
            if (club == null)
            {
                return NotFound();
            }

            var currentSeizoen = _seizoenService.GetNextSeizoen().Result;
            if (currentSeizoen == null)
            {
                return View("NoSeizoen");
            }
            if (await CheckFullMatchAsync(club.ClubId, currentSeizoen))
            {
                return View("FullMatch");
            }
            if (await CheckAbonnementAsync(club.ClubId, currentSeizoen))
            {
                return View("HasAbonnement");
            }


            var abonnement = new SelectAbonnementVM
            {
                ClubID = club.ClubId,
                StadionNaam = club.Thuisstadion.Naam,
                Naam = club.Naam,
                SeizoenID = currentSeizoen.SeizoenId,
                Seizoen = _mapper.Map<SeizoenVM>(currentSeizoen),
                Zones = new SelectList(await _zoneService.FilterById(club.Thuisstadion.StadionId), "ZoneId", "Naam")
            };

            return View(abonnement);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SelectAbonnementVM abonnementVM)
        {
            if (abonnementVM == null)
            {
                return NotFound();
            }

            var club = await _clubService.FindById(Convert.ToInt32(abonnementVM.ClubID));
            var zone = await _zoneService.FindById(Convert.ToInt32(abonnementVM.ZoneID));
            if (club == null || zone == null)
            {
                return NotFound();
            }

            if (await IsZoneFullAsync(abonnementVM))
            {
                abonnementVM.Zones = new SelectList(await _zoneService.FilterById(club.Thuisstadion.StadionId), "ZoneId", "Naam", abonnementVM.ZoneID);
                abonnementVM.Seizoen = _mapper.Map<SeizoenVM>(_seizoenService.GetNextSeizoen().Result);
                TempData["ErrorVolzetMessage"] = $"Er zijn geen plaatsen meer beschikbaar in deze zone";
                return View(abonnementVM);
            }

            var abonnement = _mapper.Map<CartAbonnementVM>(abonnementVM);
            abonnement.ClubVM = _mapper.Map<ClubVM>(club);
            abonnement.ZoneNaam = zone.Naam;
            abonnement.Prijs = zone.PrijsAbonnement;
            abonnement.DateCreated = DateTime.Now;

            if (CheckShoppingCart(abonnement))
            {
                return View("DoubleBooked");
            }

            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCart == null)
            {
                shoppingCart = new ShoppingCartVM();
            }

            if (shoppingCart.Abonnementen == null)
            {
                shoppingCart.Abonnementen = new List<CartAbonnementVM>();
            }

            shoppingCart.Abonnementen.Add(abonnement);
            HttpContext.Session.SetObject("ShoppingCart", shoppingCart);

            return RedirectToAction("Index", "ShoppingCart");
        }

        private async Task<bool> CheckAbonnementAsync(int clubId, Seizoen currentSeizoen)
        {
            var currentUserId = _userManager.GetUserId(User);
            var abonnementList = await _abonnementService.FindByStringId(currentUserId);
            return abonnementList.Any(abonnement => abonnement.ClubId == clubId && abonnement.SeizoenId == currentSeizoen.SeizoenId);
        }
        
        private async Task<bool> IsZoneFullAsync(SelectAbonnementVM abonnementVM)
        {
            var currentZone = await _zoneService.FindById(abonnementVM.ZoneID);
            int aantalAbonnementPlaatsen = (await _stoelService.GetTakenSeatsByClubID(abonnementVM.ClubID, abonnementVM.ZoneID, abonnementVM.SeizoenID)).Count();
            var matchList = await _matchService.FindByHomeClub(abonnementVM.ClubID);
            foreach (var match in matchList)
            {
                int aantalTicketPlaatsen = (await _stoelService.GetTakenSeatsByMatchID(match.MatchId, abonnementVM.ZoneID, abonnementVM.SeizoenID)).Count();
                if (currentZone.Capaciteit - (aantalAbonnementPlaatsen + aantalTicketPlaatsen) <= 0)
                {
                    return true;
                }

            }
            return false;
        }

        private async Task<bool> CheckFullMatchAsync(int clubId, Seizoen currentSeizoen)
        {
            var matchList = await _matchService.FindByHomeClub(clubId);
            foreach (var match in matchList)
            {
                if (match.SeizoenId == currentSeizoen.SeizoenId)
                {
                    int aantalVolleZones = 0;
                    var zones = await _zoneService.FilterById(match.Stadion.StadionId);
                    foreach (var zone in zones)
                    {
                        int aantalAbonnementPlaatsen = (await _stoelService.GetTakenSeatsByClubID(clubId, zone.ZoneId, currentSeizoen.SeizoenId)).Count();
                        int aantalTicketPlaatsen = (await _stoelService.GetTakenSeatsByMatchID(match.MatchId, zone.ZoneId, currentSeizoen.SeizoenId)).Count();
                        if (zone.Capaciteit - (aantalAbonnementPlaatsen + aantalTicketPlaatsen) <= 0)
                        {
                            aantalVolleZones++;
                        }
                    }
                    if (aantalVolleZones == zones.Count())
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckShoppingCart(CartAbonnementVM abonnement)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            if (shoppingCart != null)
            {
                if (shoppingCart.Abonnementen != null && shoppingCart.Abonnementen.Any(a => a.ClubID == abonnement.ClubID))
                {
                    return true;
                }
            }

            return false;

        }
    }
}
