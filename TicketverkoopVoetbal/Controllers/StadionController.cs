using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class StadionController : Controller
    {
        private readonly IService<Stadion> _stadionService;
        private readonly IService<Zone> _zoneService;
        private readonly IMapper _mapper;

        public StadionController(IService<Stadion> stadionService, IService<Zone> zoneService, IMapper mapper)
        {
            _stadionService = stadionService;
            _zoneService = zoneService;
            _mapper = mapper;
        }

        //public async Task<IActionResult> Index()  // add using System.Threading.Tasks;
        //{
        //    var list = await _stadionService.GetAll();
        //    List<StadionVM> listVM = _mapper.Map<List<StadionVM>>(list);
        //    return View(listVM);
        //}

        public async Task<IActionResult> Index()
        {
            var stadionList = await _stadionService.GetAll();
            var stadionVMList = new List<StadionVM>();

            foreach (var stadion in stadionList)
            {
                var zones = await _zoneService.FilterById(stadion.StadionId);
                int totalCapacity = zones.Sum(z => z.Capaciteit);

                var stadionVM = new StadionVM
                {
                    Naam = stadion.Naam,
                    Adres = stadion.Adres,
                    Stad = stadion.Stad,
                    TotalCapaciteit = totalCapacity
                };

                stadionVMList.Add(stadionVM);
            }

            return View(stadionVMList);
        }
    }
}
