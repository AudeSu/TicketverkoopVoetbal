namespace TicketverkoopVoetbal.ViewModels
{
    public class CreateStoelVM
    {
        public int StadionID { get; set; }

        public int ClubID { get; set; }

        public int ZoneID { get; set; }

        public int? MatchID { get; set; }
        public Boolean Bezet { get; set; }
    }
}
