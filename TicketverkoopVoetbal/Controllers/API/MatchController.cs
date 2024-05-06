using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class MatchController : Controller
    {
        private readonly IMatchService<Match> _matchService;
        private readonly IMapper _mapper;
        public MatchController(IMatchService<Match> matchService, IMapper mapper)
        {
            _matchService = matchService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of all Matches.
        /// </summary>
        /// <returns>The list of Matches.</returns>
        // GET: api/Match
        [HttpGet]
        public async Task<ActionResult<MatchVM>> Get()
        {
            try
            {
                var listMatch = await _matchService.GetAll();
                var data = _mapper.Map<List<MatchVM>>(listMatch);

                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }

        /// <summary>
        /// Get one Matches.
        /// </summary>
        /// <returns>The list of Matches.</returns>
        // GET: api/Match/{thuisploegId, uitploegId}
        [HttpGet("{thuisploegId, uitploegId}", Name = "Get")]
        public async Task<ActionResult<MatchVM>> Get(int thuisploegId, int uitploegId)
        {
            try
            {
                var listMatch = await _matchService.FindByTwoIds(thuisploegId, uitploegId);
                var data = _mapper.Map<List<MatchVM>>(listMatch);

                if (data == null)
                {
                    return NotFound();
                }
                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
