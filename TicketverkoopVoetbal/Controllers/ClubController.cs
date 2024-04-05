using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class ClubController : Controller
    {
        private IService<Club> _clubService;
        private readonly IMapper _mapper;

        public ClubController(IMapper mapper, IService<Club> clubservice)
        {
            _mapper = mapper;
            _clubService = clubservice;
        }

        public async Task<IActionResult> Index()  // add using System.Threading.Tasks;
        {
            var list = await _clubService.GetAll();
            List<ClubVM> listVM = _mapper.Map<List<ClubVM>>(list);
            return View(listVM);
        }
    }
}
