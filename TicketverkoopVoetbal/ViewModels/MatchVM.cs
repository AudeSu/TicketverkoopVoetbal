namespace TicketverkoopVoetbal.ViewModels
{
    public class MatchVM
    {
        public string? StadionNaam { get; set; }
        public string? ThuisploegNaam { get; set; }
        public string? UitploegNaam { get; set; }
        public DateOnly? Datum {  get; set; }
        public TimeSpan? Startuur {  get; set; }
    
    }
}
