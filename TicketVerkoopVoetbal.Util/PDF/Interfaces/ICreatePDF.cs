using TicketverkoopVoetbal.Domains.Entities;

namespace TicketVerkoopVoetbal.Util.PDF.Interfaces
{
    public interface ICreatePDF
    {
        MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, /*List<Abonnement> abonnementen,*/ string logoPath, string headerPath, AspNetUser user);
    }
}
