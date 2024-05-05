using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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
        private IEmailSend _emailSend;
        private ICreatePDF _createPDF;
        private IUserService<AspNetUser> _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;

        public ShoppingCartController(
            IEmailSend emailsend,
            ICreatePDF createPDF,
            IUserService<AspNetUser> userService,
            UserManager<IdentityUser> userManager,
            IWebHostEnvironment hostingEnvironment,
            IMapper mapper)
        {
            _emailSend = emailsend;
            _createPDF = createPDF;
            _userService = userService;
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
            var currentUser = await _userService.FindByStringId(GetCurrentUserId());
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            if (cartList == null
                || cartList.Carts == null
                || cartList.Carts.Count == 0
                || currentUser == null)
            {
                return RedirectToAction("Index", "Match");
            }

            try
            {
                string pdfFile = "Factuur" + DateTime.Now.Year;
                var pdfFileName = $"{pdfFile}_{Guid.NewGuid()}.pdf";
                var tickets = cartList.Carts;

                var ticketList = new List<Ticket>();
                foreach (var item in tickets)
                {
                    Ticket ticket = _mapper.Map<Ticket>(item);
                    ticketList.Add(ticket);
                }
                // Het pad naar de map waarin het logo zich bevindt
                string logoPath = Path.Combine(_hostingEnvironment.WebRootPath, "images", "logo.png");
                var pdfDocument = _createPDF.CreatePDFDocumentAsync(ticketList, logoPath);

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
                HttpContext.Session.Remove("ShoppingCart");
                return View("Thanks");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
            }
            return View();
        }

        public IActionResult DeleteTicket(int? matchId)
        {
            if (matchId == null)
            {
                return NotFound();
            }
            ShoppingCartVM? cartList = HttpContext.Session.GetObject<ShoppingCartVM>("ShoppingCart");
            CartTicketVM? itemToRemove = cartList?.Carts?.FirstOrDefault(r => r.MatchId == matchId);

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

        private string GetCurrentUserId()
        {
            var user = _userManager.GetUserAsync(User).Result;

            if (user != null)
            {
                return user.Id;
            }
            else
            {
                return "fout";
            }
        }
    }
}
