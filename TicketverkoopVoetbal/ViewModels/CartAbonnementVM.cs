namespace TicketverkoopVoetbal.ViewModels
{
    public class CartAbonnementVM : SelectAbonnementVM
    {
        public string ZoneNaam { get; set; }
        public decimal Prijs { get; set; }
        public ClubVM ClubVM { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}
