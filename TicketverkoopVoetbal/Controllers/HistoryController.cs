using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;

namespace TicketverkoopVoetbal.Controllers
{
    public class HistoryController : Controller
    {
        private readonly ITicketService<Ticket> _ticketService;
        private readonly IAbonnementService<Abonnement> _abonnementService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryController(
            ITicketService<Ticket> ticketservice,
            IAbonnementService<Abonnement> abonnementservice,
            IMapper mapper,
            UserManager<IdentityUser> userManager
            )
        {
            _ticketService = ticketservice;
            _abonnementService = abonnementservice;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            try
            {
                var currentUserID = _userManager.GetUserId(User);
                var ticketList = await _ticketService.FindByStringId(currentUserID);
                var AbonnementList = await _abonnementService.FindByStringId(currentUserID);
                HistoryVM historyVM = new HistoryVM();
                historyVM.CartTicketVMs = _mapper.Map<List<CartTicketVM>>(ticketList);
                historyVM.AbonnementVMs = _mapper.Map<List<CartAbonnementVM>>(AbonnementList);

                return View(historyVM);
            }
            catch (Exception ex)
            {
                return View(ex);
            }
        }
    }
}
