using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class MatchVM
    {
        public int MatchId { get; set; }

        public int StadionId { get; set; }
        public string? StadionNaam { get; set; }
        public string? ThuisploegNaam { get; set; }
        public string? UitploegNaam { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Datum {  get; set; }

        public TimeSpan? Startuur { get; set; }

    }
}
