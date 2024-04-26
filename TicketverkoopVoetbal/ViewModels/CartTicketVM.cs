namespace TicketverkoopVoetbal.ViewModels
{
    public class CartTicketVM
    {
        public int? MatchId { get; set; }

        public int? ZoneId { get; set; }

        public string ZoneNaam { get; set; }

        public MatchVM matchVM { get; set; }

        public int Aantal { get; set; }
        public decimal Prijs { get; set; }
        public System.DateTime DateCreated { get; set; }

    }
}
