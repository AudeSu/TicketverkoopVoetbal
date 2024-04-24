using TicketverkoopVoetbal.Domains.Entities;
using TicketVerkoopVoetbal.Util.PDF.Interfaces;

namespace TicketVerkoopVoetbal.Util.PDF
{
    public class CreatePDF : ICreatePDF
    {
        public MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, string logoPath)
        {
            throw new NotImplementedException();
        }
    }
}
