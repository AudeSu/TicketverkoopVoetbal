using Microsoft.AspNetCore.Mvc.Rendering;

namespace TicketverkoopVoetbal.ViewModels
{
    public class SelectAbonnementVM
    {
        public string GebruikerID { get; set; }
        public int StoeltjeID { get; set; }
        public int ZoneID { get; set; }
        public int ClubID { get; set; }
        public string Naam { get; set; }
        public string StadionNaam { get; set; }
        public int SeizoenID { get; set; }
        public SeizoenVM Seizoen { get; set; }
        public IEnumerable<SelectListItem>? Zones { get; set; }
    }
}
