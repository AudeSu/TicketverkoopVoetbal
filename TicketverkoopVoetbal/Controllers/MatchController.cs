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
    public class MatchController : Controller
    {

        private IService<Match> _matchService;
        private IService<Club> _clubService;
        private readonly IMapper _mapper;

        public MatchController(IMapper mapper, IService<Match> matchservice, IService<Club> clubService)
        {
            _mapper = mapper;
            _matchService = matchservice;
            _clubService = clubService;
        }

        public async Task<ActionResult> Index()
        {
            var matchlist = await _matchService.GetAll();

            ClubMatchVM clubmatchVM = new ClubMatchVM();
            clubmatchVM.Matches = _mapper.Map<List<MatchVM>>(matchlist);
            clubmatchVM.Clubs = new SelectList(
                await _clubService.GetAll(), "ClubId", "Naam");

            return View(clubmatchVM);
        }

        [HttpPost]
        public async Task<IActionResult> Index(ClubMatchVM entity)
        {
            if (entity == null)
            {
                return NotFound();
            }
            try
            {
                var matchlist = await _matchService.FilterById(Convert.ToInt32(entity.ClubNumber));
                   

                ClubMatchVM clubmatchVM = new ClubMatchVM();
                clubmatchVM.Matches = _mapper.Map<List<MatchVM>>(matchlist);
                clubmatchVM.Clubs =
                new SelectList(await _clubService.GetAll(), "ClubId", "Naam", entity.ClubNumber);

                return View(clubmatchVM);


            }
            catch (Exception ex)
            {
                Debug.WriteLine("errorlog" + ex.Message);
            }

            return View(entity);
        }

        //public async Task<IActionResult> Select(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    Match? match = await _matchService.FindById(Convert.ToInt32(id));

        //    if (match != null)
        //    {
        //        //CartVM item = new CartVM
        //        //{
        //        //    MatchId = match.MatchId,
        //        //    StadionNaam = match.Stadion.Naam,
        //        //    ThuisploegNaam = match.Thuisploeg.Naam,
        //        //    UitploegNaam = match.Uitploeg.Naam,
        //        //    Datum = match.Datum,
        //        //    Startuur = match.Startuur,
        //        //    Aantal = 1,
        //        //    Prijs = 15,
        //        //    DateCreated = DateTime.Now
                    
        //        //};

        //        //ShoppingCartVM? shopping;

        //        //// var objComplex = HttpContext.Session.GetObject<ShoppingCartVM>("ComplexObject");
        //        //if (HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart") != null)
        //        //{
        //        //    shopping = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
        //        //}
        //        //else
        //        //{
        //        //    shopping = new ShoppingCartVM();
        //        //    shopping.Carts = new List<CartVM>();
        //        //}
        //        //shopping?.Carts?.Add(item);


        //        //HttpContext.Session.SetObject("ShoppingCart", shopping);

        //    }
        //    return RedirectToAction("Index", "Ticket");

        //}

    }


}
