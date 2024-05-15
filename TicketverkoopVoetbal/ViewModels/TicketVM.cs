using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class TicketVM
    {
        public int TicketID { get; set; }
        public int MatchID { get; set; }
        public int ZoneID { get; set; }
        public int StoeltjeID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Datum { get; set; }
        public TimeSpan? Startuur { get; set; }
        public string ZoneNaam { get; set; }
    }
}
