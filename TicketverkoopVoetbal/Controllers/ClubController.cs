using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class ClubController : Controller
    {
        private readonly IService<Club> _clubService;
        private readonly IMapper _mapper;

        public ClubController(IService<Club> clubservice, IMapper mapper)
        {
            _clubService = clubservice;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var clubs = await _clubService.GetAll();
            var clubVMs = new List<ClubVM>();
            foreach (var club in clubs)
            {
                var clubVM = _mapper.Map<ClubVM>(club);
                clubVMs.Add(clubVM);
            }

            return View(clubVMs);
        }
    }
}
