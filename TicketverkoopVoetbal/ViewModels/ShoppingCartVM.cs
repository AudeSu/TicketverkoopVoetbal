namespace TicketverkoopVoetbal.ViewModels
{
    public class ShoppingCartVM
    {
        public List<CartTicketVM>? Carts { get; set; }

        public List<CartAbonnementVM>? Abonnementen { get; set; }

        public decimal Total => ComputeTotalValue();

        // Method to compute the total value
        public decimal ComputeTotalValue() =>
            Carts?.Sum(e => e.Prijs) ?? 0;
    }
}
