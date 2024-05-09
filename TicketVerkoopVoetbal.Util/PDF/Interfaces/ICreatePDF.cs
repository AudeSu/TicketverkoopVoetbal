using TicketverkoopVoetbal.Domains.Entities;

namespace TicketVerkoopVoetbal.Util.PDF.Interfaces
{
    public interface ICreatePDF
    {
        MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, string logoPath, string email);
    }
}
