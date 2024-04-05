using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class MatchController : Controller
    {

        private IService<Match> _matchService;
        private readonly IMapper _mapper;

        public MatchController(IMapper mapper, IService<Match> matchservice)
        {
            _mapper = mapper;
            _matchService = matchservice;
        }

        public async Task<IActionResult> Index()  // add using System.Threading.Tasks;
        {
            var list = await _matchService.GetAll();
            List<MatchVM> listVM = _mapper.Map<List<MatchVM>>(list);
            return View(listVM);


        }

    }


}
