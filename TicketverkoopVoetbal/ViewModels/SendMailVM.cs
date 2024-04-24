using System.ComponentModel.DataAnnotations;

namespace TicketverkoopVoetbal.ViewModels
{
    public class SendMailVM
    {
        [Required, Display(Name = "Uw email"), EmailAddress]
        public string? Email { get; set; }
        [Required]
        [DataType(DataType.MultilineText)]
        public string? Message { get; set; }
    }
}
