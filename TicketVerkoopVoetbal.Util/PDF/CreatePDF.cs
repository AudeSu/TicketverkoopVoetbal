using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
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

                string qrContent = "https://example.com";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5); // Grootte van 20 pixels
                iText.Layout.Element.Image qrCodeImageElement = new
                iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(qrCodeImageElement);

                document.Add(new Paragraph("Factuur").SetFontSize(20));
                document.Add(new Paragraph("Factuurnummer: 001").SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA)).SetFontSize(16).SetFontColor(ColorConstants.BLUE));
                document.Add(new Paragraph("Datum: " + DateTime.Now.ToShortDateString()));
                document.Add(new Paragraph(""));

                //Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
                //table.AddHeaderCell("Product");
                //table.AddHeaderCell("Prijs per stuk");
                //table.AddHeaderCell("Totaal");
                //decimal totalPrice = 0;
                //foreach (var ticket in tickets)
                //{
                //    table.AddCell(ticket.GebruikersId);
                //    table.AddCell(ticket.Zone.Prijs.ToString("C"));
                //    decimal totalProductPrice = ticket.Zone.Prijs * ticket.Zone.Aantal;
                //    table.AddCell(totalProductPrice.ToString("C"));
                //    totalPrice += totalProductPrice;
                //}
                //document.Add(table);

                //Paragraph paragraph = new Paragraph("Totaal: " + totalPrice.ToString("C"))
                //     .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                //     .SetFontSize(12)
                //     .SetFontColor(ColorConstants.BLACK)
                //     .SetTextAlignment(TextAlignment.LEFT);
                //document.Add(paragraph);

                document.Close();
                return new MemoryStream(stream.ToArray());
            }
        }
        // This method is for converting bitmap into a byte array
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
