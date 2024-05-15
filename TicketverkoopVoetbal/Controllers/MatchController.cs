using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.IdentityModel.Tokens;
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

        public MatchController(IMatchService<Match> matchservice, IService<Club> clubService, IMapper mapper)
        {
            _matchService = matchservice;
            _clubService = clubService;
            _mapper = mapper;
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
                IEnumerable<Match> matchlist;
                if (entity.ClubNumber == 0)
                {
                    // Als "Alle Clubs" geselecteerd is (waarde 0), haal alle wedstrijden op
                    matchlist = await _matchService.GetAll();
                }
                else
                {
                    // Haal wedstrijden op voor de geselecteerde club
                    matchlist = await _matchService.FilterById(Convert.ToInt32(entity.ClubNumber));
                }

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
