namespace TicketverkoopVoetbal.ViewModels
{
    public class CartTicketVM : SelectTicketVM
    {
        public int StoeltjeID { get; set; }
        public string GebruikersID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ZoneNaam { get; set; }
        public System.DateTime DateCreated { get; set; }
    }
}
