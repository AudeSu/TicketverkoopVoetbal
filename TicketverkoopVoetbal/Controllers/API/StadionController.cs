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
    public class StadionController : Controller
    {
        private IService<Stadion> _stadionService;
        private readonly IMapper _mapper;
        public StadionController(IMapper mapper, IService<Stadion> stadionService)
        {
            _mapper = mapper;
            _stadionService = stadionService;
        }
        /// <summary>
        /// Get the list of all Stadions.
        /// </summary>
        /// <returns>The list of Stadions.</returns>
        // GET: api/Stadion
        [HttpGet, Authorize]
        public async Task<ActionResult<StadionVM>> Get()
        {
            try
            {
                var listStadion = await _stadionService.GetAll();
                var data = _mapper.Map<List<StadionVM>>(listStadion);

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
