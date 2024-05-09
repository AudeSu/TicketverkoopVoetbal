using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketverkoopVoetbal.ViewModels
{
    public class SelectTicketVM
    {
        public int MatchId { get; set; }
        public int ZoneId { get; set; }
        public MatchVM matchVM { get; set; }
        public IEnumerable<SelectListItem>? Zones { get; set; }

        public List<HotelVM> HotelLijst { get; set; }
        public int Aantal { get; set; }

        public int VrijePlaatsen { get; set; }
        public decimal Prijs { get; set; }
    }
}
