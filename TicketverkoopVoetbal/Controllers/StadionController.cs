using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class StadionController : Controller
    {
        private IService<Stadion> _stadionService;
        private readonly IMapper _mapper;

        public StadionController(IMapper mapper, IService<Stadion> stadionservice)
        {
            _mapper = mapper;
            _stadionService = stadionservice;
        }

        public async Task<IActionResult> Index()  // add using System.Threading.Tasks;
        {
            var list = await _stadionService.GetAll();
            List<StadionVM> listVM = _mapper.Map<List<StadionVM>>(list);
            return View(listVM);
        }
    }
}
