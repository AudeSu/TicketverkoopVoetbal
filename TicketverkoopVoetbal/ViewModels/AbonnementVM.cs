using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketverkoopVoetbal.ViewModels
{
    public class AbonnementVM
    {
        public string GebruikerID { get; set; }
        public int StoeltjeId { get; set; }
        public int ZoneId { get; set; }
        public int ClubId { get; set; }
        public string Naam { get; set; }
        public string StadionNaam { get; set; }
        public int SeizoenId { get; set; }
        public SeizoenVM Seizoen { get; set; }
        public IEnumerable<SelectListItem>? Zones { get; set; }

    }
}
