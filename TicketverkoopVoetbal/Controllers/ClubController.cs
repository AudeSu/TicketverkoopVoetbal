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

        public ClubController(IMapper mapper, IService<Club> clubservice)
        {
            _mapper = mapper;
            _clubService = clubservice;
        }

        public async Task<IActionResult> Index()  // add using System.Threading.Tasks;
        {
            var clubs = await _clubService.GetAll();
            var clubVMs = new List<ClubVM>();
            foreach (var club in clubs)
            {
                var clubVM = _mapper.Map<ClubVM>(club);
                clubVM.LogoPath = GetClubLogoPath(club.ClubId);
                clubVMs.Add(clubVM);
            }

            return View(clubVMs);
        }

        private string GetClubLogoPath(int clubId)
        {
            switch (clubId)
            {
                case 2:
                    return "~/images/Royale_Union_Saint-Gilloise_logo.png";
                case 3:
                    return "~/images/RSC_Anderlecht_logo.png";
                case 4:
                    return "~/images/Club_Brugge_logo.png";
                case 5:
                    return "~/images/KRC_Genk_logo.png";
                case 6:
                    return "~/images/Royal_Antwerp_Football_Club_logo.png";
                case 7:
                    return "~/images/Cercle_Brugge_logo.png";
                default:
                    return "";
            }
        }
    }
}
