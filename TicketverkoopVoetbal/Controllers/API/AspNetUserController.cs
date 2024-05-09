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
        private readonly IUserService<AspNetUser> _userService;
        private readonly IMapper _mapper;
        public AspNetUserController(IUserService<AspNetUser> userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
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
