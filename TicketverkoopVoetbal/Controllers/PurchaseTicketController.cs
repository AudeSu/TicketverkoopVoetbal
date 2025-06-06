﻿using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Diagnostics;
using TicketverkoopVoetbal.Domains;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services;
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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _Configure;
        private string? BaseUrl;

        public PurchaseTicketController(
            IMatchService<Match> matchservice,
            IService<Zone> zoneService,
            IStoelService<Stoeltje> stoelService,
            ITicketService<Ticket> ticketService,
            UserManager<IdentityUser> userManager,
            IMapper mapper,
            IConfiguration configuration)
        {
            _matchService = matchservice;
            _zoneService = zoneService;
            _stoelService = stoelService;
            _ticketService = ticketService;
            _userManager = userManager;
            _mapper = mapper;
            _Configure = configuration;
            BaseUrl = _Configure.GetValue<string>("APIURL");
        }

        [Authorize]
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
            SelectTicketVM ticketVM = new SelectTicketVM();
            ticketVM.MatchId = match.MatchId;
            ticketVM.matchVM = matchVM;
            ticketVM.Zones = new SelectList(await _zoneService.FilterById(match.StadionId), "ZoneId", "Naam");
            ticketVM.HotelLijst = await GetHotelsAsync(match.Stadion.Stad);

            if (CheckTicketHistory(ticketVM))
            {
                return View("DoubleBooked");
            }
            if (GetTicketAmount(ticketVM) == 4)
            {
                return View("MaxTickets");
            }

            return View(ticketVM);
        }

        [HttpPost]
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
                ticketVM.Prijs = zone.Prijs;
                ticketVM.Zones =
                new SelectList(await _zoneService.FilterById(Convert.ToInt16(match.StadionId)), "ZoneId", "Naam", ticketVM.ZoneId);
                HttpContext.Session.SetObject("TicketVM", ticketVM);

                ticketVM.VrijePlaatsen = VrijePlaatsen(ticketVM);
                if (ticketVM.Aantal != 0)
                {
                    if(VrijePlaatsen(ticketVM) < ticketVM.Aantal)
                    {
                        TempData["ErrorVolzetMessage"] = $"Er zijn nog maar {ticketVM.VrijePlaatsen} beschikbaar in deze zone";

                        return View(ticketVM);
                    }

                    if (CheckTicketAmount(ticketVM))
                    {

                        return RedirectToAction("Names");

                    }
                    else
                    {
                        var ticketAmount = GetTicketAmount(ticketVM);
                        TempData["ErrorMessage"] = $"Momenteel heb je al {ticketAmount} ticket(s) voor deze wedstrijd. U kan nog maar {ticketAmount - 4} ticket(s) voor deze wedstrijd boeken";

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


        public Boolean CheckTicketAmount(SelectTicketVM ticketVM)
        {
            var underMaxTickets = true;
            var currentUserID = _userManager.GetUserId(User);
            var ticketList = _ticketService.FindPerUser(currentUserID, ticketVM.MatchId);

            if (ticketList.Result.Count() + ticketVM.Aantal > 4)
            {
                return false;
            }

            return underMaxTickets;
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
                nameVMs.Add(new TicketNameVM());
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
                            Eigenaar = nameVMs[i].Name,
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
