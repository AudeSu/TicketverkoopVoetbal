namespace TicketverkoopVoetbal.ViewModels
{
    public class CartAbonnementVM
    {
        public int ClubId { get; set; }
        public string GebruikerID { get; set; }
        public int StoeltjeId { get; set; }
        public int SeizoenID { get; set; }
        public int ZoneId { get; set; }
        public string ZoneNaam { get; set; }
        public decimal Prijs { get; set; }
        public ClubVM clubVM { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}
