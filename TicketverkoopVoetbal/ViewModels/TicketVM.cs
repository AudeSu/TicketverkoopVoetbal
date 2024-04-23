using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketverkoopVoetbal.ViewModels
{
    public class TicketVM
    {
        public int? MatchId { get; set; }
        public int? ZoneId { get; set; }
        public MatchVM matchVM { get; set; }
        public IEnumerable<SelectListItem>? Zones { get; set; }
        public int Aantal { get; set; }
        public decimal Prijs { get; set; }
    }
}
