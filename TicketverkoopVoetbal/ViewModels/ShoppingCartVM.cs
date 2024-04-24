namespace TicketverkoopVoetbal.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartVM>? Carts { get; set; }

        public decimal Total => ComputeTotalValue();

        // Method to compute the total value
        public decimal ComputeTotalValue() =>
            Carts?.Sum(e => e.Prijs * e.Aantal) ?? 0;


    }

    public class CartVM
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

