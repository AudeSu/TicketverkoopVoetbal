using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
        private IService<Match> _matchService;
        private readonly IMapper _mapper;
        public MatchController(IMapper mapper, IService<Match> matchService)
        {
            _mapper = mapper;
            _matchService = matchService;
        }

        // TODO: Lijst van alle wedstrijden tussen twee ploegen. Wanneer de api wordt opgeroepen wordt id van het thuis‐ en uitploeg meegegeven.

        /// <summary>
        /// Get the list of all Matches.
        /// </summary>
        /// <returns>The list of Matches.</returns>
        // GET: api/Match
        [HttpGet, Authorize]
        public async Task<ActionResult<MatchVM>> Get()
        {
            try
            {
                var listMatch = await _matchService.GetAll();
                var data = _mapper.Map<List<MatchVM>>(listMatch);

                if (data == null)
                {// Als de gegevens niet worden gevonden, retourneer een 404 Not Found-status
                    return NotFound();
                }
                // Retourneer de gegevens als alles goed is verlopen
                // HTTP-statuscode 200
                return Ok(data);
            }
            catch (Exception ex)
            {
                // Als er een fout optreedt, retourneer een 500 Internal Server Error-status
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }
}
