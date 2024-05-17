namespace TicketverkoopVoetbal.ViewModels
{
    public class HistoryAbonnementVM
    {
        public int ClubId { get; set; }
        public string GebruikerID { get; set; }
        public int StoeltjeID { get; set; }
        public int SeizoenID { get; set; }
        public SeizoenVM seizoenVM { get; set; }
        public string ZoneNaam { get; set; }
        public string clubNaam { get; set; }
        public decimal Prijs {  get; set; }
    }
}
