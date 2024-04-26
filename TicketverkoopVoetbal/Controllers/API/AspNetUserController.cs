using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AspNetUserController : Controller
    {
        private IService<AspNetUser> _userService;
        private readonly IMapper _mapper;
        public AspNetUserController(IMapper mapper, IService<AspNetUser> userService)
        {
            _mapper = mapper;
            _userService = userService;
        }
        /// <summary>
        /// Get the list of all Users.
        /// </summary>
        /// <returns>The list of Users.</returns>
        // GET: api/AspNetUser
        [HttpGet]
        public async Task<ActionResult<AspNetUserVM>> Get()
        {
            try
            {
                var listUser = await _userService.GetAll();
                var data = _mapper.Map<List<AspNetUserVM>>(listUser);

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
