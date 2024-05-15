using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class MatchVM
    {
        public int MatchId { get; set; }
        public int ClubId { get; set; }
        public int StadionId { get; set; }
        public int SeizoenID { get; set; }
        public string? StadionNaam { get; set; }
        public string? ThuisploegNaam { get; set; }
        public string? UitploegNaam { get; set; }
        [DataType(DataType.Date)]
        public DateTime? Datum {  get; set; }
        public TimeSpan? Startuur { get; set; }
        public string ThuisploegLogoPath { get; set; }
        public string UitploegLogoPath { get; set; }
    }
}
