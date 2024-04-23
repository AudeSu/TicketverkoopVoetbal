namespace TicketverkoopVoetbal.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartVM>? Carts { get; set; }

        public decimal ComputeTotalValue() =>
       Carts.Sum(e => e.Prijs * e.Aantal);


    }

    public class CartVM
    {
        public int? MatchId { get; set; }

        public MatchVM matchVM { get; set; }

        public int Aantal { get; set; }
        public decimal Prijs { get; set; }
        public System.DateTime DateCreated { get; set; }

    }
}

