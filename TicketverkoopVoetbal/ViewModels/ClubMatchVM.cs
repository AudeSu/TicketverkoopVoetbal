using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketverkoopVoetbal.ViewModels
{
    public class ClubMatchVM
    {
        public int? ClubID { get; set; }
        public IEnumerable<SelectListItem>? Clubs { get; set; }
        public IEnumerable<MatchVM>? Matches { get; set; }

    }
}
