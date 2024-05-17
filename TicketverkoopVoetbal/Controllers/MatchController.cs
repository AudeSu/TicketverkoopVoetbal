using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class MatchController : Controller
    {
        private readonly IMatchService<Match> _matchService;
        private readonly IService<Club> _clubService;
        private readonly IService<Zone> _zoneService;
        private readonly IStoelService<Stoeltje> _stoelService;
        private readonly IMapper _mapper;

        public MatchController(
            IMatchService<Match> matchservice,
            IService<Club> clubService,
            IService<Zone> zoneService,
            IStoelService<Stoeltje> stoelService,
            IMapper mapper)
        {
            _matchService = matchservice;
            _clubService = clubService;
            _stoelService = stoelService;
            _zoneService = zoneService;
            _mapper = mapper;
        }

        public async Task<ActionResult> Index()
        {
            try
            {
                var futureMatches = await _matchService.GetFutureMatches();
                var matchVMs = _mapper.Map<List<MatchVM>>(futureMatches);
                var clubmatchVM = new ClubMatchVM
                {
                    Matches = matchVMs,
                    Clubs = new SelectList(await _clubService.GetAll(), "ClubId", "Naam")
                };

                return View(clubmatchVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return StatusCode(500);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(ClubMatchVM entity)
        {
            if (entity == null)
            {
                return NotFound();
            }

            try
            {
                IEnumerable<Match> matchlist;

                if (entity.ClubID == 0)
                {
                    matchlist = await _matchService.GetFutureMatches();
                }
                else
                {
                    matchlist = await _matchService.GetFutureMatchesById(Convert.ToInt32(entity.ClubID));
                }

                var matchVMs = _mapper.Map<List<MatchVM>>(matchlist);
                var clubmatchVM = new ClubMatchVM
                {
                    Matches = matchVMs,
                    Clubs = new SelectList(await _clubService.GetAll(), "ClubId", "Naam", entity.ClubID)
                };

                return View(clubmatchVM);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                return StatusCode(500);
            }
        }
    }
}
