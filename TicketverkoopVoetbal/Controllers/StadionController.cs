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

        public StadionController(IService<Stadion> stadionService, IService<Zone> zoneService)
        {
            _stadionService = stadionService;
            _zoneService = zoneService;
        }

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
                    Capaciteit = totalCapacity,
                    FotoPath = stadion.PhotoPath
                };

                stadionVMList.Add(stadionVM);
            }

            return View(stadionVMList);
        }
    }
}
