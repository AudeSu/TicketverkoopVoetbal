using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class TicketNameVM
    {
        [Required(ErrorMessage = "Gelieve uw voornaam in te vullen.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Gelieve uw achternaam in te vullen.")]
        public string LastName { get; set; }
    }
}
