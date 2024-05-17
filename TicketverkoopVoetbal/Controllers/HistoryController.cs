using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Areas.Data;
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
        private readonly IMatchService<Match> _matchService;
        private readonly ISeizoenService<Seizoen> _seizoenService;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public HistoryController(
            ITicketService<Ticket> ticketservice,
            IAbonnementService<Abonnement> abonnementservice,
            IStoelService<Stoeltje> stoelservice,
            IMatchService<Match> matchservice,
            ISeizoenService<Seizoen> seizoenservice,
            IMapper mapper,
            UserManager<ApplicationUser> userManager
        )
        {
            _ticketService = ticketservice;
            _abonnementService = abonnementservice;
            _stoelService = stoelservice;
            _matchService = matchservice;
            _seizoenService = seizoenservice;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Index()
        {
            var currentUserID = _userManager.GetUserId(User);
            if (string.IsNullOrEmpty(currentUserID))
            {
                return NotFound();
            }

            try
            {
                var ticketList = await _ticketService.FindByStringId(currentUserID);
                var AbonnementList = await _abonnementService.FindByStringId(currentUserID);

                var historyVM = new HistoryVM
                {
                    TicketVMs = _mapper.Map<List<TicketVM>>(ticketList),
                    AbonnementVMs = _mapper.Map<List<HistoryAbonnementVM>>(AbonnementList)
                };

                historyVM.TicketVMs.Sort((t1, t2) => DateTime.Compare(t1.Datum.GetValueOrDefault(), t2.Datum.GetValueOrDefault()));

                foreach (var item in historyVM.AbonnementVMs)
                {
                    var seizoen = await _seizoenService.FindById(item.SeizoenID);
                    item.seizoenVM = _mapper.Map<SeizoenVM>(seizoen);
                    var stoel = await _stoelService.FindById(item.StoeltjeID);
                    item.ZoneNaam = stoel!.Zone.Naam;
                    item.Prijs = stoel!.Zone.PrijsAbonnement;
                }
                foreach (var item in historyVM.TicketVMs)
                {
                    var match = await _matchService.FindById(item.MatchID);
                    item.matchVM = _mapper.Map<MatchVM>(match);
                    var stoel = await _stoelService.FindById(item.StoeltjeID);
                    item.ZoneNaam = stoel!.Zone.Naam;
                    item.Prijs = stoel!.Zone.PrijsTicket;
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
            if (ticketID <= 0)
            {
                return NotFound();
            }

            var ticket = await _ticketService.FindById(ticketID);
            if (ticket == null)
            {
                return NotFound();
            }

            var stoel = await _stoelService.FindById(ticket.StoeltjeId);
            if (stoel != null)
            {
                stoel.Bezet = false;
                await _stoelService.Update(stoel);
                await _ticketService.Delete(ticket);
            }
            return RedirectToAction("Index");
        }
    }
}
