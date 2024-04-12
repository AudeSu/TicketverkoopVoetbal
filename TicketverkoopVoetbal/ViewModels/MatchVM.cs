using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class MatchVM
    {
        public string? StadionNaam { get; set; }
        public string? ThuisploegNaam { get; set; }
        public string? UitploegNaam { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Datum {  get; set; }

        public TimeSpan? Startuur { get; set; }

    }
}
