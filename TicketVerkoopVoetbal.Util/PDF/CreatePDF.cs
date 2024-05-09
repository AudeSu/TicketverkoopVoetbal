using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using QRCoder;
using System.Drawing;
using TicketverkoopVoetbal.Domains.Entities;
using TicketVerkoopVoetbal.Util.PDF.Interfaces;

namespace TicketVerkoopVoetbal.Util.PDF
{
    public class CreatePDF : ICreatePDF
    {
        //public MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, string logoPath)
        //{
        //    MemoryStream memoryStream = new MemoryStream();
        //    PdfWriter pdfWriter = new PdfWriter(memoryStream);
        //    PdfDocument pdfDocument = new PdfDocument(pdfWriter);
        //    Document document = new Document(pdfDocument);

        //    // Voeg logo toe
        //    iText.Layout.Element.Image logo = new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath));
        //    logo.SetWidth(100);
        //    document.Add(logo);

        //    // Titel van het ticket
        //    Paragraph title = new Paragraph("Ticket Details")
        //        .SetTextAlignment(TextAlignment.CENTER)
        //        .SetFontSize(18);
        //    document.Add(title);

        //    // Voeg ticketinformatie toe
        //    foreach (var ticket in tickets)
        //    {
        //        document.Add(new Paragraph($"Ticket ID: {ticket.TicketId}"));
        //        // document.Add(new Paragraph($"Match: {ticket.MatchId}"));
        //        // document.Add(new Paragraph($"Datum: {ticket.Match.Startuur.ToString("dd-MM-yyyy HH:mm")}"));
        //        //document.Add(new Paragraph($"Zone: {ticket.Zone}"));
        //        //document.Add(new Paragraph($"Stoeltje: {ticket.StoeltjeId}"));

        //        // Voeg QR-code toe met ticketinformatie
        //        string qrText = "Ticket ID:";
        //        QRCodeGenerator qrGenerator = new QRCodeGenerator();
        //        QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);
        //        QRCode qrCode = new QRCode(qrCodeData);
        //        Bitmap qrCodeBitmap = qrCode.GetGraphic(5);
        //        iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeBitmap))).SetHorizontalAlignment(HorizontalAlignment.CENTER);
        //        document.Add(qrCodeImageElement);

        //        // Voeg een lege regel toe tussen de tickets
        //        document.Add(new AreaBreak());
        //    }

        //    document.Close();
        //    return memoryStream;
        //}

        
        public MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, string logoPath)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter writer = new PdfWriter(stream);
                PdfDocument pdf = new PdfDocument(writer);
                iText.Layout.Document document = new iText.Layout.Document(pdf);

                ImageData imageData = ImageDataFactory.Create(logoPath);
                iText.Layout.Element.Image image = new iText.Layout.Element.Image(imageData).SetMaxWidth(100);
                document.Add(image);



                document.Add(new Paragraph("Factuur").SetFontSize(20));
                document.Add(new Paragraph("Factuurnummer: 001").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)).SetFontSize(16).SetFontColor(ColorConstants.BLUE));
                document.Add(new Paragraph("Datum: " + DateTime.Now.ToShortDateString()));
                document.Add(new Paragraph(""));

                foreach (var ticket in tickets)
                {
                    document.Add(new Paragraph($"Ticket ID: {ticket.TicketId}"));
                    document.Add(new Paragraph($"Match: {ticket.MatchId}"));
                    //document.Add(new Paragraph($"Datum: {ticket.Match.Startuur.ToString("dd-MM-yyyy HH:mm")}"));
                    //document.Add(new Paragraph($"Zone: {ticket.Zone.Naam}"));
                    document.Add(new Paragraph($"Stoeltje: {ticket.StoeltjeId}"));

                    // Voeg QR-code toe met ticketinformatie
                    string qrContent = "https://example.com";
                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(5); // Grootte van 5 pixels
                    iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                    document.Add(qrCodeImageElement);

                    // Voeg een lege regel toe tussen de tickets
                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));
                }

                document.Close();
                return new MemoryStream(stream.ToArray());
            }
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
    }
}
