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
        private ITicketService<Ticket> _ticketService;
        private IAbonnementService<Abonnement> _abonnementService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryController(
            IMapper mapper,
            ITicketService<Ticket> ticketservice,
            IAbonnementService<Abonnement> abonnementservice,
            UserManager<IdentityUser> userManager
            )
        {
            _mapper = mapper;
            _ticketService = ticketservice;
            _abonnementService = abonnementservice;
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
