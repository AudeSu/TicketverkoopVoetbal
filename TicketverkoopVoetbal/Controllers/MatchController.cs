using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService<Match> _matchService;
        private readonly IService<Club> _clubService;
        private readonly IMapper _mapper;

        public MatchController(IMapper mapper, IMatchService<Match> matchservice, IService<Club> clubService)
        {
            _mapper = mapper;
            _matchService = matchservice;
            _clubService = clubService;
        }

        public async Task<ActionResult> Index()
        {
            var matchlist = await _matchService.GetAll();
            var matchVMs = GetFutureMatches(matchlist);

            var clubmatchVM = new ClubMatchVM
            {
                Matches = matchVMs,
                Clubs = new SelectList(await _clubService.GetAll(), "ClubId", "Naam")
            };

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
                var matchVMs = GetFutureMatches(matchlist);

                var clubmatchVM = new ClubMatchVM
                {
                    Matches = matchVMs,
                    Clubs = new SelectList(await _clubService.GetAll(), "ClubId", "Naam", entity.ClubNumber)
                };

                return View(clubmatchVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
            }

            return View(entity);
        }

        private List<MatchVM> GetFutureMatches(IEnumerable<Match> matches)
        {
            var currentDate = DateTime.Today;

            var futureMatches = matches
                .Where(m => m.Datum >= currentDate)
                .OrderBy(m => m.Datum)
                .ThenBy(m => m.Startuur)
                .ToList();

            var matchVMs = _mapper.Map<List<MatchVM>>(futureMatches);

            return matchVMs;
        }
    }
}
