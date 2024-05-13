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
        private readonly IStoelService<Stoeltje> _stoelService;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;

        public HistoryController(
            ITicketService<Ticket> ticketservice,
            IAbonnementService<Abonnement> abonnementservice,
            IStoelService<Stoeltje> stoelservice,
            IMapper mapper,
            UserManager<IdentityUser> userManager
            )
        {
            _ticketService = ticketservice;
            _abonnementService = abonnementservice;
            _stoelService = stoelservice;
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
                historyVM.TicketVMs = _mapper.Map<List<TicketVM>>(ticketList);
                historyVM.AbonnementVMs = _mapper.Map<List<HistoryAbonnementVM>>(AbonnementList);
                // Sorteer TicketVMs op basis van de Datum van de wedstrijd
                historyVM.TicketVMs.Sort((t1, t2) => DateTime.Compare(t1.Datum.GetValueOrDefault(), t2.Datum.GetValueOrDefault()));
                foreach (var item in historyVM.AbonnementVMs)
                {
                    item.ZoneNaam = _stoelService.FindById(item.StoeltjeID).Result.Zone.Naam;
                }

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
