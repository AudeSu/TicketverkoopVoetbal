using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TicketverkoopVoetbal.Areas.Data;
using TicketverkoopVoetbal.Domains.Entities;
using TicketverkoopVoetbal.Extensions;
using TicketverkoopVoetbal.Services.Interfaces;
using TicketverkoopVoetbal.ViewModels;
using TicketVerkoopVoetbal.Util.Mail.Interfaces;
using TicketVerkoopVoetbal.Util.PDF.Interfaces;

namespace TicketverkoopVoetbal.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IEmailSend _emailSend;
        private readonly ICreatePDF _createPDF;
        private readonly IAbonnementService<Abonnement> _abonnementService;
        private readonly IStoelService<Stoeltje> _stoelService;
        private readonly ITicketService<Ticket> _ticketService;
        private readonly IMatchService<Match> _matchService;
        private readonly IService<Zone> _zoneService;
        private readonly IUserService<AspNetUser> _UserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IEmailSend emailsend,
            ICreatePDF createPDF,
            IAbonnementService<Abonnement> abonnementService,
            IStoelService<Stoeltje> stoelService,
            ITicketService<Ticket> ticketService,
            IMatchService<Match> matchService,
            IService<Zone> zoneService,
            IUserService<AspNetUser> userService,
            UserManager<ApplicationUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _emailSend = emailsend;
            _createPDF = createPDF;
            _abonnementService = abonnementService;
            _stoelService = stoelService;
            _ticketService = ticketService;
            _matchService = matchService;
            _zoneService = zoneService;
            _UserService = userService;
            _userManager = userManager;
            _hostingEnvironment = hostingEnvironment;
            _mapper = mapper;
        }

        [Authorize]
        public IActionResult Index()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");

            return View(cartList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Purchase()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            var ASPcurrentUser = await _UserService.FindByStringId(currentUser.Id);
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (cartList == null || currentUser == null)
            {
                return RedirectToAction("Index", "Match");
            }
            if (cartList.Abonnement != null)
            {
                await CreateAbonnement(cartList.Abonnement);
            }
            if (cartList.Carts != null)
            {
                await CreateTicket(cartList.Carts);
            }

            try
            {
                string pdfFile = "Factuur" + DateTime.Now.Year;
                var pdfFileName = $"{pdfFile}_{Guid.NewGuid()}.pdf";

                if (cartList.Carts != null)
                {
                    var tickets = cartList.Carts;

                    var ticketList = new List<Ticket>();
                    foreach (var item in tickets)
                    {
                        Ticket ticket = _mapper.Map<Ticket>(item);
                        ticket.Match = _matchService.FindById(item.MatchID).Result;
                        ticket.Zone = _zoneService.FindById(item.ZoneID).Result;
                        ticketList.Add(ticket);
                    }
                    // Het pad naar de map waarin het logo zich bevindt
                    string logoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "Website_logo.png");
                    string headerPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "PDF Header.jpg");
                    var pdfDocument = _createPDF.CreatePDFDocumentAsync(ticketList, logoPath, headerPath, ASPcurrentUser);

                    // Als de map pdf nog niet bestaat in de wwwroot map,
                    // maak deze dan aan voordat je het PDF-document opslaat.
                    string pdfFolderPath = Path.Combine(_hostingEnvironment.WebRootPath, "pdf");
                    Directory.CreateDirectory(pdfFolderPath);
                    //Combineer het pad naar de wwwroot map met het gewenste subpad en bestandsnaam voor het PDF-document.
                    string filePath = Path.Combine(pdfFolderPath, pdfFileName);
                    // Opslaan van de MemoryStream naar een bestand
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        pdfDocument.WriteTo(fileStream);
                    }

                    _emailSend.SendEmailAttachmentAsync(currentUser.Email, pdfDocument, pdfFileName);
                
                }
                HttpContext.Session.Remove("ShoppingCart");
                return View("Thanks");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        private async Task CreateAbonnement(CartAbonnementVM cartAbonnementVM)
        {
            CreateStoelVM stoelVM = new CreateStoelVM();
            stoelVM.ZoneID = cartAbonnementVM.ZoneId;
            stoelVM.StadionID = cartAbonnementVM.clubVM.StadionID;
            stoelVM.ClubID = cartAbonnementVM.ClubId;
            stoelVM.MatchID = null;
            stoelVM.Bezet = true;
            Stoeltje stoel = _mapper.Map<Stoeltje>(stoelVM);
            await _stoelService.Add(stoel);

            cartAbonnementVM.GebruikerID = _userManager.GetUserId(User);
            cartAbonnementVM.StoeltjeId = stoel.StoeltjeId;
            //voorlopig hardcoded want ik weet niet hoe
            cartAbonnementVM.SeizoenID = 1;
            Abonnement abonnement = _mapper.Map<Abonnement>(cartAbonnementVM);
            await _abonnementService.Add(abonnement);
        }


        private async Task CreateTicket(List<CartTicketVM> ticketLijst)
        {
            for (int i = 0; i < ticketLijst.Count; i++)
            {
                var currentTicket = ticketLijst[i];
                CreateStoelVM stoelVM = new CreateStoelVM();
                stoelVM.ZoneID = currentTicket.ZoneID;
                stoelVM.StadionID = currentTicket.matchVM.StadionId;
                stoelVM.ClubID = currentTicket.matchVM.ClubId;
                stoelVM.MatchID = currentTicket.MatchID;
                stoelVM.Bezet = true;
                Stoeltje stoel = _mapper.Map<Stoeltje>(stoelVM);
                await _stoelService.Add(stoel);

                currentTicket.GebruikersID = _userManager.GetUserId(User);
                currentTicket.StoeltjeID = stoel.StoeltjeId;
                Ticket ticket = _mapper.Map<Ticket>(currentTicket);
                await _ticketService.Add(ticket);
            }
        }


        public IActionResult DeleteTicket(int? matchId)
        {
            if (matchId == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            CartTicketVM? itemToRemove = cartList?.Carts?.FirstOrDefault(r => r.MatchID == matchId);

            if (itemToRemove != null)
            {
                cartList?.Carts?.Remove(itemToRemove);
                HttpContext.Session.SetObject("ShoppingCart", cartList);
            }
            return RedirectToAction("Index");
        }

        public IActionResult DeleteAbonnement()
        {
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            CartAbonnementVM? itemToRemove = cartList?.Abonnement;

            if (itemToRemove != null)
            {
                cartList.Abonnement = null;
                HttpContext.Session.SetObject("ShoppingCart", cartList);
            }
            return RedirectToAction("Index");
        }
    }
}
