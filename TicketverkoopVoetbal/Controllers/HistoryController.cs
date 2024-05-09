using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace TicketverkoopVoetbal.Controllers
{
    public class HistoryController : Controller
    {
        private ITicketService<Ticket> _ticketService;
        private IAbonnementService<Abonnement> _abonnementService;
        private IService<Stoeltje> _stoelService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryController(
            IMapper mapper,
            ITicketService<Ticket> ticketservice,
            IAbonnementService<Abonnement> abonnementservice,
            IService<Stoeltje> stoelservice,
            UserManager<IdentityUser> userManager
            )
        {
            _mapper = mapper;
            _ticketService = ticketservice;
            _abonnementService = abonnementservice;
            _stoelService = stoelservice;
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
                historyVM.TicketVMs = _mapper.Map<List<TicketVM>>(ticketList);
                historyVM.AbonnementVMs = _mapper.Map<List<CartAbonnementVM>>(AbonnementList);

                return View(historyVM);
            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }


        public async Task<IActionResult> DeleteTicket(int ticketID)
        {
            if (ticketID == null)
            {
                return NotFound();
            }
            Ticket ticket = await _ticketService.FindById(Convert.ToInt32(ticketID));

            if (ticket != null)
            {
                Stoeltje stoel = await _stoelService.FindById(ticket.StoeltjeId);
                stoel.Bezet = false;
                await _stoelService.Update(stoel);
                await _ticketService.Delete(ticket);
            }
            return RedirectToAction("Index");
        }
    }
}
