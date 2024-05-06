using AutoMapper;
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
        private readonly IService<Stadion> _stadionService;
        private readonly IMapper _mapper;
        public StadionController(IService<Stadion> stadionService, IMapper mapper)
        {
            _stadionService = stadionService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get the list of all Stadions.
        /// </summary>
        /// <returns>The list of Stadions.</returns>
        // GET: api/Stadion
        [HttpGet]
        public async Task<ActionResult<StadionVM>> Get()
        {
            try
            {
                var listStadion = await _stadionService.GetAll();
                var data = _mapper.Map<List<StadionVM>>(listStadion);

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
