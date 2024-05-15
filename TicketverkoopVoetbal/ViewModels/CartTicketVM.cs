namespace TicketverkoopVoetbal.ViewModels
{
    public class CartTicketVM
    {
        public int MatchID { get; set; }

        public int ZoneID { get; set; }

        public int StoeltjeID { get; set; }

        public string GebruikersID { get; set; }

        //public string Eigenaar { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ZoneNaam { get; set; }

        public MatchVM matchVM { get; set; }

        public decimal Prijs { get; set; }
        public System.DateTime DateCreated { get; set; }

    }
}
