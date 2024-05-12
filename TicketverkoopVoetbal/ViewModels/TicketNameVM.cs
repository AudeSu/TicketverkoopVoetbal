using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class TicketNameVM
    {
        [Required(ErrorMessage = "Gelieve uw naam in te vullen.")]
        public string Name { get; set; }
    }
}
