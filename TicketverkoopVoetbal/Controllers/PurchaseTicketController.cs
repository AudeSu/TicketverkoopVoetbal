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
        private readonly string? BaseUrl;
        private readonly int? MaxTickets;

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
            BaseUrl = _configuration.GetValue<string>("APIURL");
            MaxTickets = _configuration.GetValue<int>("MaxTickets");
        }

        [Authorize]
        public async Task<IActionResult> Index(int? id)
        {
            var match = await _matchService.FindById(Convert.ToInt32(id));
            if (id == null || match == null)
            {
                return NotFound();
            }
            if(FullMatch(Convert.ToInt32(id)))
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
            MatchVM matchVM = new();
            matchVM = _mapper.Map<MatchVM>(match);
            if (match == null)
            {
                return NotFound();
            }
            SelectTicketVM ticketVM = new SelectTicketVM();
            ticketVM.MatchId = match.MatchId;
            ticketVM.matchVM = matchVM;
            ticketVM.Zones = new SelectList(await _zoneService.FilterById(match.StadionId), "ZoneId", "Naam");
            ticketVM.HotelLijst = await GetHotelsAsync(match.Stadion.Stad);

            if (CheckTicketHistory(ticketVM))
            {
                return View("DoubleBooked");
            }
            if (GetTicketAmount(ticketVM) == MaxTickets)
            {
                return View("MaxTickets");
            }
            if (CheckAbonnement(ticketVM))
            {
                return View("HasAbonnement");
            }
            if (CheckShoppingCartDate(ticketVM))
            {
                TempData["ErrorDoubleBooked"] = $"U heeft al tickets op deze match in u ShoppingCart staan, u kunt maar 1 match per dag";
                return View(ticketVM);
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
                var zone = await _zoneService.FindById(Convert.ToInt32(ticketVM.ZoneId));
                var match = await _matchService.FindById(Convert.ToInt32(ticketVM.MatchId));
                MatchVM matchVM = new MatchVM();
                matchVM = _mapper.Map<MatchVM>(match);
                ticketVM.matchVM = matchVM;
                ticketVM.Prijs = zone.PrijsTicket;
                ticketVM.HotelLijst = await GetHotelsAsync(match.Stadion.Stad);
                ticketVM.Zones =
                new SelectList(await _zoneService.FilterById(Convert.ToInt16(match.StadionId)), "ZoneId", "Naam", ticketVM.ZoneId);
                HttpContext.Session.SetObject("TicketVM", ticketVM);

                ticketVM.VrijePlaatsen = VrijePlaatsen(ticketVM);
                if (ticketVM.Aantal != 0)
                {
                    if (VrijePlaatsen(ticketVM) < ticketVM.Aantal)
                    {
                        TempData["ErrorVolzetMessage"] = $"Er zijn nog maar {ticketVM.VrijePlaatsen} plaatsen beschikbaar in deze zone";

                        return View(ticketVM);
                    }
                    var aantalCartTickets = CheckShoppingCart(ticketVM);
                    if (aantalCartTickets + ticketVM.Aantal > MaxTickets)
                    {
                        TempData["ErrorTeveelTickets"] = $"U heeft al {aantalCartTickets} in u shoppingCart en kunt dus nog maar {MaxTickets-aantalCartTickets} tickets kopen.";
                        return View(ticketVM);
                    }
                    if (CheckShoppingCartDate(ticketVM))
                    {
                        TempData["ErrorDoubleBooked"] = $"U heeft al tickets op deze dag in u ShoppingCart staan.";
                        return View(ticketVM);
                    }

                    if (CheckTicketAmount(ticketVM))
                    {

                        return RedirectToAction("Names");

                    }
                    else
                    {
                        var ticketAmount = GetTicketAmount(ticketVM);
                        TempData["ErrorMessage"] = $"Momenteel heb je al {ticketAmount} ticket(s) voor deze wedstrijd. U kan nog maar {MaxTickets - ticketAmount} ticket(s) voor deze wedstrijd boeken";

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

        public int GetTicketAmount(SelectTicketVM ticketVM)
        {
            var aantalTickets = 0;
            var currentUserID = _userManager.GetUserId(User);
            var ticketList = _ticketService.FindByStringId(currentUserID);
            foreach (var ticket in ticketList.Result)
            {
                if (ticket.MatchId == ticketVM.MatchId)
                {
                    aantalTickets++;
                }
            }
            return aantalTickets;
        }

        public Boolean CheckTicketHistory(SelectTicketVM ticketVM)
        {
            var hasTicket = false;
            var currentUserID = _userManager.GetUserId(User);
            var ticketList = _ticketService.FindByStringId(currentUserID);

            foreach (var ticket in ticketList.Result)
            {
                if (ticket.Match.Datum == ticketVM.matchVM.Datum && ticket.MatchId != ticketVM.MatchId)
                {
                    hasTicket = true;
                }
            }

            return hasTicket;
        }




        public Boolean CheckAbonnement(SelectTicketVM ticketVM)
        {
            var hasAbonnement = false;
            var currentUserID = _userManager.GetUserId(User);
            var AbonnementList = _abonnementService.FindByStringId(currentUserID);

            foreach (var abonnement in AbonnementList.Result)
            {
                if (abonnement.ClubId == ticketVM.matchVM.ClubId)
                {
                    hasAbonnement = true;
                }
            }

            return hasAbonnement;
        }


        public Boolean CheckTicketAmount(SelectTicketVM ticketVM)
        {
            var underMaxTickets = true;
            var currentUserID = _userManager.GetUserId(User);
            var ticketList = _ticketService.FindPerUser(currentUserID, ticketVM.MatchId);

            if (ticketList.Result.Count() + ticketVM.Aantal > MaxTickets)
            {
                return false;
            }

            return underMaxTickets;
        }

        public Boolean FullMatch(int id)
        {
            Boolean isFull = false;
            var match = _matchService.FindById(id).Result;
            if(match != null)
            {
                int aantalVolleZones = 0;
                var zones = _zoneService.FilterById(match.Stadion.StadionId).Result;
                foreach (var zone in zones)
                {
                    int aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(id, zone.ZoneId, match.SeizoenId).Result.Count();
                    int aantalTicketPlaatsen = _stoelService.GetTakenSeatsByMatchID(match.MatchId, zone.ZoneId).Result.Count();
                    if (zone.Capaciteit - (aantalAbonnementPlaatsen + aantalTicketPlaatsen) <= 0)
                    {
                        aantalVolleZones++;
                    }
                }

                if (aantalVolleZones == zones.Count())
                {
                    return isFull = true;
                }
            }
            return isFull;

        }

        public int CheckShoppingCart(SelectTicketVM ticketVM)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            int aantalTickets = 0;
            if (shoppingCart != null)
            {
                if (shoppingCart.Carts != null)
                {
                    foreach (var item in shoppingCart.Carts)
                    {
                        if (item.MatchID == ticketVM.MatchId)
                        { 
                            aantalTickets++;
                        }
                    }
                }
            }

            return aantalTickets;
        }

        public Boolean CheckShoppingCartDate(SelectTicketVM ticketVM)
        {
            var shoppingCart = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            Boolean DoubleBooked = false;
            if (shoppingCart != null)
            {
                if (shoppingCart.Carts != null)
                {
                    foreach (var item in shoppingCart.Carts)
                    {
                        var currentmatch = _matchService.FindById(item.MatchID).Result;
                        if (item.MatchID != ticketVM.MatchId && currentmatch.Datum == ticketVM.matchVM.Datum)
                        {
                            return DoubleBooked = true;
                        }
                    }
                }
            }

            return DoubleBooked;
        }

        public int VrijePlaatsen(SelectTicketVM ticketVM)
        {
            var currentZone = _zoneService.FindById(Convert.ToInt32(ticketVM.ZoneId)).Result;

            int aantalAbonnementPlaatsen = _stoelService.GetTakenSeatsByClubID(ticketVM.matchVM.ClubId, ticketVM.ZoneId, ticketVM.matchVM.SeizoenID).Result.Count();
            int aantalticketPlaatsen = _stoelService.GetTakenSeatsByMatchID(ticketVM.MatchId, ticketVM.ZoneId).Result.Count();

            return currentZone.Capaciteit - (aantalAbonnementPlaatsen + aantalticketPlaatsen);
        }

        private async Task<List<HotelVM>> GetHotelsAsync(string StadNaam)
        {
            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("Referer", "FullStack_Opdracht");

                var search = $"/search?q=hotels+in+{StadNaam},Belgium";
                var format = "json";
                var limit = "3";

                var url = $"{BaseUrl}{search}&format={format}&limit={limit}";

                using (var response = await httpClient.GetAsync(url))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var hotels = JsonConvert.DeserializeObject<List<Hotel>>(apiResponse);

                    // Map the Hotel objects to HotelVM objects
                    return _mapper.Map<List<HotelVM>>(hotels);
                }
            }
        }

        [Authorize]
        public IActionResult Names()
        {
            int? aantal = HttpContext.Session.GetObject<SelectTicketVM>("TicketVM").Aantal;

            List<TicketNameVM> nameVMs = new List<TicketNameVM>();
            for (int i = 0; i < aantal; i++)
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
            if (ModelState.IsValid)
            {
                var ticketVM = HttpContext.Session.GetObject<SelectTicketVM>("TicketVM");
                if (ticketVM == null)
                {
                    return NotFound();
                }

                Match? match = await _matchService.FindById(Convert.ToInt32(ticketVM.MatchId));
                Zone? zone = await _zoneService.FindById(Convert.ToInt32(ticketVM.ZoneId));
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
                if (shopping.Carts == null)
                {
                    shopping.Carts = new List<CartTicketVM>();
                }

                if (match != null)
                {
                    for (int i = 0; i < ticketVM.Aantal; i++)
                    {
                        CartTicketVM item = new CartTicketVM
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
                        shopping?.Carts?.Add(item);
                    }
                    HttpContext.Session.SetObject("ShoppingCart", shopping);
                }
                return RedirectToAction("Index", "ShoppingCart");
            }
            else
            {
                return View("Names", nameVMs);
            }
        }
    }
}
