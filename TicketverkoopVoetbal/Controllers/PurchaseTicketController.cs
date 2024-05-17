using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using TicketverkoopVoetbal.Areas.Data;
using TicketverkoopVoetbal.Domains;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class PurchaseTicketController : Controller
    {
        private readonly IMatchService<Match> _matchService;
        private readonly IService<Zone> _zoneService;
        private readonly IStoelService<Stoeltje> _stoelService;
        private readonly ITicketService<Ticket> _ticketService;
        private readonly IAbonnementService<Abonnement> _abonnementService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly string? _baseUrl;
        private readonly int _maxTickets;

        public PurchaseTicketController(
            IMatchService<Match> matchservice,
            IService<Zone> zoneService,
            IStoelService<Stoeltje> stoelService,
            ITicketService<Ticket> ticketService,
            IAbonnementService<Abonnement> abonnementService,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            _matchService = matchservice;
            _zoneService = zoneService;
            _stoelService = stoelService;
            _ticketService = ticketService;
            _abonnementService = abonnementService;
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
            _baseUrl = _configuration.GetValue<string>("APIURL");
            _maxTickets = _configuration.GetValue<int>("MaxTickets");
        }

        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            if (!id.HasValue)
            {
                return NotFound();
            }

            var match = await _matchService.FindById(id.Value);
            if (match == null)
            {
                return NotFound();
            }

            if (IsMatchFull(id.Value))
            {
                return View("FullMatch");
            }

            if (match.Datum >= DateTime.Now.AddMonths(1))
            {
                return View("FutureMatch");
            }

            if (match.Datum < DateTime.Now && match.Startuur < DateTime.Now.TimeOfDay)
            {
                return View("PastMatch");
            }

            var matchVM = _mapper.Map<MatchVM>(match);
            var ticketVM = new SelectTicketVM
            {
                MatchId = match.MatchId,
                matchVM = matchVM,
                Zones = new SelectList(await _zoneService.FilterById(match.StadionId), "ZoneId", "Naam"),
                HotelLijst = await GetHotelsAsync(match.Stadion.Stad)
            };


            if (IsDoubleBooked(ticketVM) || IsInShoppingCart(ticketVM))
            {
                return View("DoubleBooked");
            }

            if (GetTicketAmount(ticketVM) >= _maxTickets)
            {
                return View("MaxTickets");
            }

            if (HasAbonnement(ticketVM))
            {
                return View("HasAbonnement");
            }

            return View(ticketVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(SelectTicketVM ticketVM)
        {
            if (ticketVM == null)
            {
                return NotFound();
            }

            try
            {
                var zone = await _zoneService.FindById(ticketVM.ZoneId);
                var match = await _matchService.FindById(ticketVM.MatchId);
                if (zone == null || match == null)
                {
                    return NotFound();
                }
                ticketVM.matchVM = _mapper.Map<MatchVM>(match);
                ticketVM.Prijs = zone.PrijsTicket;
                ticketVM.HotelLijst = await GetHotelsAsync(match.Stadion.Stad);
                ticketVM.Zones = new SelectList(await _zoneService.FilterById(match.StadionId), "ZoneId", "Naam", ticketVM.ZoneId);
                ticketVM.VrijePlaatsen = GetAvailableSeats(ticketVM);


                HttpContext.Session.SetObject("TicketVM", ticketVM);

                if (ticketVM.Aantal != 0)
                {
                    if (GetAvailableSeats(ticketVM) < ticketVM.Aantal)
                    {
                        TempData["ErrorVolzetMessage"] = $"Er zijn nog maar {ticketVM.VrijePlaatsen} plaatsen beschikbaar in deze zone";
                        return View(ticketVM);
                    }

                    var aantalCartTickets = GetCartTicketCount(ticketVM);
                    if (aantalCartTickets + ticketVM.Aantal > _maxTickets)
                    {
                        TempData["ErrorTeveelTickets"] = $"U heeft al {aantalCartTickets} tickets in uw shoppingCart en kunt dus nog maar {_maxTickets - aantalCartTickets} tickets kopen.";
                        return View(ticketVM);
                    }

                    if (ticketVM.VrijePlaatsen - aantalCartTickets < ticketVM.Aantal)
                    {
                        TempData["ErrorTeveelTickets"] = $" U heeft al {aantalCartTickets} tickets in uw shoppingCart en kunt dus nog maar  {ticketVM.VrijePlaatsen - aantalCartTickets} tickets kopen.";
                        return View(ticketVM);
                    }

                    if (HasReachedMaxTicketAmount(ticketVM))
                    {
                        return RedirectToAction("Names");
                    }

                    else
                    {
                        var ticketAmount = GetTicketAmount(ticketVM);
                        TempData["ErrorMessage"] = $"Momenteel heb je al {ticketAmount} ticket(s) voor deze wedstrijd. U kan nog maar {_maxTickets - ticketAmount} ticket(s) voor deze wedstrijd boeken";
                        return View(ticketVM);
                    }
                }
                return View(ticketVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("errorlog" + ex.Message);
            }
            return View(ticketVM);
        }

        private async Task<List<HotelVM>> GetHotelsAsync(string StadNaam)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Referer", "FullStack_Opdracht");
            var url = $"{_baseUrl}/search?q=hotels+in+{StadNaam},Belgium&format=json&limit=3";

            var response = await httpClient.GetAsync(url);
            var apiResponse = await response.Content.ReadAsStringAsync();
            var hotels = JsonConvert.DeserializeObject<List<Hotel>>(apiResponse);

            return _mapper.Map<List<HotelVM>>(hotels);
        }

        public int GetTicketAmount(SelectTicketVM ticketVM)
        {
            int aantalTickets = 0;
            var currentUserID = _userManager.GetUserId(User);
            var tickets = _ticketService.FindByStringId(currentUserID).Result;
            foreach (var ticket in tickets)
            {
                if (ticket.MatchId == ticketVM.MatchId)
                {
                    aantalTickets++;
                }
            }
            return aantalTickets;
        }

        public int GetAvailableSeats(SelectTicketVM ticketVM)
        {
            var currentZone = _zoneService.FindById(ticketVM.ZoneId).Result;
            var aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(ticketVM.matchVM.ClubId, ticketVM.ZoneId, ticketVM.matchVM.SeizoenID).Result.Count();
            int aantalticketPlaatsen = _stoelService.GetTakenSeatsByMatchID(ticketVM.MatchId, ticketVM.ZoneId, ticketVM.matchVM.SeizoenID).Result.Count();

            return currentZone.Capaciteit - (aantalAbonnementPlaatsen + aantalticketPlaatsen);
        }

        public bool IsDoubleBooked(SelectTicketVM ticketVM)
        {
            var currentUserID = _userManager.GetUserId(User);
            var ticketList = _ticketService.FindByStringId(currentUserID).Result;

            foreach (var ticket in ticketList)
            {
                if (ticket.Match.Datum == ticketVM.matchVM.Datum && ticket.MatchId != ticketVM.MatchID)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasAbonnement(SelectTicketVM ticketVM)
        {
            var currentUserID = _userManager.GetUserId(User);
            var abonnementen = _abonnementService.FindByStringId(currentUserID).Result;

            foreach (var abonnement in abonnementen)
            {
                if (abonnement.ClubId == ticketVM.matchVM.ClubId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasReachedMaxTicketAmount(SelectTicketVM ticketVM)
        {
            var currentUserID = _userManager.GetUserId(User);
            var tickets = _ticketService.FindPerUser(currentUserID, ticketVM.MatchId).Result;


            return tickets.Count() + ticketVM.Aantal < _maxTickets;
        }

        private bool IsMatchFull(int id)
        {
            var match = _matchService.FindById(id).Result;
            if (match != null)
            {
                int aantalVolleZones = 0;
                var zones = _zoneService.FilterById(match.Stadion.StadionId).Result;
                foreach (var zone in zones)
                {
                    int aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(id, zone.ZoneId, match.SeizoenId).Result.Count();
                    int aantalTicketPlaatsen = _stoelService.GetTakenSeatsByMatchID(match.MatchId, zone.ZoneId, match.SeizoenId).Result.Count();
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
            return false;
        }

        public int GetCartTicketCount(SelectTicketVM ticketVM)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            int aantalTickets = 0;
            if (shoppingCart != null && shoppingCart.Carts != null)
            {
                foreach (var item in shoppingCart.Carts)
                {
                    if (item.MatchID == ticketVM.MatchId)
                    {
                        aantalTickets++;

                    }
                }
            }

            return aantalTickets;
        }

        public bool IsInShoppingCart(SelectTicketVM ticketVM)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingCart != null && shoppingCart.Carts != null)
            {
                foreach (var item in shoppingCart.Carts)
                {
                    var currentmatch = _matchService.FindById(item.MatchID).Result;
                    if (item.MatchID != ticketVM.MatchId && currentmatch.Datum == ticketVM.matchVM.Datum)
                    {
                        return true;

                    }
                }
            }

            return false;
        }

        [Authorize]
        public IActionResult Names()
        {
            var ticketVM = HttpContext.Session.GetObject<SelectTicketVM>("TicketVM");
            if (ticketVM == null)
            {
                return NotFound();
            }

            var nameVMs = new List<TicketNameVM>();
            for (int i = 0; i < ticketVM.Aantal; i++)
            {
                if (i != 0)
                {
                    nameVMs.Add(new TicketNameVM());
                }
                else
                {
                    nameVMs.Add(new TicketNameVM
                    {
                        FirstName = _userManager.GetUserAsync(User).Result.FirstName,
                        LastName = _userManager.GetUserAsync(User).Result.LastName
                    });
                }
            }

            return View(nameVMs);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Names(List<TicketNameVM> nameVMs)
        {
            if (!ModelState.IsValid)
            {
                return View("Names", nameVMs);
            }

            var ticketVM = HttpContext.Session.GetObject<SelectTicketVM>("TicketVM");
            if (ticketVM == null)
            {
                return NotFound();
            }

            var match = await _matchService.FindById(ticketVM.MatchId);
            var zone = await _zoneService.FindById(ticketVM.ZoneId);
            if (match == null || zone == null)
            {
                return NotFound();
            }

            var shoppingcart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (shoppingcart == null)
            {
                shoppingcart = new ShoppingCartVM();
            }
            if (shoppingcart.Carts == null)
            {
                shoppingcart.Carts = new List<CartTicketVM>();
            }

            for (int i = 0; i < ticketVM.Aantal; i++)
            {
                var item = new CartTicketVM
                {
                    MatchID = ticketVM.MatchId,
                    matchVM = _mapper.Map<MatchVM>(match),
                    ZoneID = ticketVM.ZoneId,
                    ZoneNaam = zone.Naam,
                    FirstName = nameVMs[i].FirstName,
                    LastName = nameVMs[i].LastName,
                    Prijs = ticketVM.Prijs,
                    DateCreated = DateTime.Now
                };
                shoppingcart?.Carts?.Add(item);
            }
            HttpContext.Session.SetObject("ShoppingCart", shoppingcart);

            return RedirectToAction("Index", "ShoppingCart");
        }
    }
}
