using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using Newtonsoft.Json;
using System.Configuration;
using System.Diagnostics;
using System.Diagnostics.Metrics;
using TicketverkoopVoetbal.Domains;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;
using static Azure.Core.HttpHeader;

namespace TicketverkoopVoetbal.Controllers
{
    public class PurchaseTicketController : Controller
    {
        private IMatchService<Match> _matchService;
        private IService<Zone> _zoneService;
        private readonly IMapper _mapper;
        private IConfiguration _Configure;
        private string? BaseUrl;

        public PurchaseTicketController(IMapper mapper, IMatchService<Match> matchservice, IService<Zone> zoneService, IConfiguration configuration)
        {
            _mapper = mapper;
            _matchService = matchservice;
            _zoneService = zoneService;
            _Configure = configuration;
            BaseUrl = _Configure.GetValue<string>("APIURL");
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
                HttpContext.Session.SetObject("TicketVM", ticketVM);
                ticketVM.HotelLijst = await GetHotelsAsync(match.Stadion.Stad);
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

            int? aantal = HttpContext.Session.GetObject<TicketVM>("TicketVM").Aantal;

            List<TicketNameVM> nameVMs = new List<TicketNameVM>();
            for (int i = 0; i < aantal; i++)
            {
                nameVMs.Add(new TicketNameVM());
            }

            return View(nameVMs);
        }

        [HttpPost]
        public async Task<IActionResult> Select(List<TicketNameVM> nameVMs)
        {
            var ticketVM = HttpContext.Session.GetObject<TicketVM>("TicketVM");
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
                        MatchId = ticketVM.MatchId,
                        matchVM = _mapper.Map<MatchVM>(match),
                        ZoneId = ticketVM.ZoneId,
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
    }
}
