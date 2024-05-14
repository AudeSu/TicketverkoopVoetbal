using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
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
        public MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, string logoPath, string email)
        {
            MemoryStream stream = new MemoryStream();
            PdfWriter writer = new PdfWriter(stream);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            // Header
            Paragraph logoParagraph = new Paragraph()
                .SetMarginRight(50)
                .Add(new iText.Layout.Element.Image(ImageDataFactory.Create(logoPath)).ScaleToFit(120, 120));

            Paragraph headerParagraph = new Paragraph("Factuur")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
                .SetFontSize(40);

            Paragraph companyInfoParagraph = new Paragraph()
                .SetMarginLeft(50)
                .Add("Ticketverkoop Voetbal").SetBold()
                .Add("\nDoorniksesteenweg 145")
                .Add("\n8500 Kortrijk, België")
                .Add("\nTel: +32 56 26 41 30");

            document.Add(new Paragraph()
                .Add(logoParagraph)
                .Add(headerParagraph)
                .Add(companyInfoParagraph));

            // horizontale lijn
            document.Add(new LineSeparator(new SolidLine(1f)));

            // Body
            document.Add(new Paragraph()
                .Add("\n")
                .Add("Naar: " + GetCustomerNameFromEmail(email)).SetBold().SetFontSize(20)
                .Add("\nFactuurdatum: " + DateTime.Now.ToShortDateString())
                .Add("\nFactuurnummer: " + DateTime.Now.ToString("MMddHHmmss"))
                .Add(new Text("\n\nInformatie over de tickets:").SetBold())
                .Add("\nBedankt voor uw aankoop van de tickets! Om toegang te krijgen tot het stadion, raden we u aan om dit document af te drukken. Bij uw bezoek aan het stadion, laat de QR-code samen met uw identiteitsbewijs zien aan het toegangspersoneel. Op deze manier wordt u probleemloos toegelaten tot de wedstrijd.\n\n"));

            // Tabel
            Table table = new Table(UnitValue.CreatePercentArray(3)).UseAllAvailableWidth();
            table.AddHeaderCell("Thuisploeg");
            table.AddHeaderCell("Uitploeg");
            table.AddHeaderCell("Tarief");
            decimal totalPrice = 0;
            foreach (var ticket in tickets)
            {
                //table.AddCell(ticket.Match.Thuisploeg.Naam);
                //table.AddCell(ticket.Match.Uitploeg.Naam);
                //table.AddCell(ticket.Zone.Prijs.ToString("C"));
                //totalPrice += ticket.Zone.Prijs;
            }
            document.Add(table);

            // Praragraaf met totaal
            Paragraph paragraph = new Paragraph("Totaal: " + totalPrice.ToString("C"))
                 .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                 .SetFontSize(12)
                 .SetFontColor(ColorConstants.BLACK)
                 .SetTextAlignment(TextAlignment.RIGHT);
            document.Add(paragraph);

            foreach (var ticket in tickets)
            {
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                document.Add(new Paragraph($"Ticket ID: {ticket.TicketId}"));
                document.Add(new Paragraph($"Match: {ticket.MatchId}"));
                //document.Add(new Paragraph($"Datum: {ticket.Match.Startuur:dd-MM-yyyy HH:mm}"));
                //document.Add(new Paragraph($"Zone: {ticket.Zone.Naam}"));
                document.Add(new Paragraph($"Stoeltje: {ticket.StoeltjeId}"));

                // Voeg QR-code toe met ticketinformatie
                string qrContent = "https://example.com";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5);
                iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                document.Add(qrCodeImageElement);
            }

            document.Close();
            return new MemoryStream(stream.ToArray());
        }

        private static string GetCustomerNameFromEmail(string email)
        {
            int atIndex = email.IndexOf('@');
            if (atIndex != -1)
            {
                string customerName = email.Substring(0, atIndex);
                string[] words = customerName.Replace('.', ' ').Split(' ', StringSplitOptions.RemoveEmptyEntries);

                for (int i = 0; i < words.Length; i++)
                {
                    if (!string.IsNullOrEmpty(words[i]))
                    {
                        words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                    }
                }
                customerName = string.Join(" ", words);

                return customerName;
            }
            else
            {
                return email;
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

// CHATGPT hihihihih

//namespace TicketVerkoopVoetbal.Util.PDF
//{
//    public class CreatePDF : ICreatePDF
//    {
//        public MemoryStream CreatePDFDocumentAsync(
//            Customer customer, 
//            List<Ticket> tickets, 
//            string companyLogoPath, 
//            string companyName, 
//            string companyAddress, 
//            string companyPhone, 
//            string invoiceNumber, 
//            DateTime invoiceDate)
//        {
//            using (MemoryStream stream = new MemoryStream())
//            {

//                // Ticket Table
//                Table ticketTable = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
//                ticketTable.AddHeaderCell("Match");
//                ticketTable.AddHeaderCell("Aantal");
//                ticketTable.AddHeaderCell("Tarief");
//                ticketTable.AddHeaderCell("Totaal");

//                decimal totalAmount = 0;
//                foreach (var ticket in tickets)
//                {
//                    ticketTable.AddCell(ticket.Match.Thuisploeg + " VS " + ticket.Match.Uitploeg);
//                    ticketTable.AddCell(ticket.Quantity.ToString());
//                    ticketTable.AddCell(ticket.Zone.Prijs.ToString("C"));
//                    decimal totalPrice = ticket.Quantity * ticket.Zone.Prijs;
//                    ticketTable.AddCell(totalPrice.ToString("C"));
//                    totalAmount += totalPrice;
//                }

//                document.Add(ticketTable);

//                // Total
//                document.Add(new Paragraph("\nTotaal: " + totalAmount.ToString("C"))
//                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD))
//                    .SetTextAlignment(TextAlignment.RIGHT));

//                // Footer
//                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

//                // Generate ticket pages
//                foreach (var ticket in tickets)
//                {
//                    document.Add(new Image(ImageDataFactory.Create(companyLogoPath)).SetHorizontalAlignment(HorizontalAlignment.CENTER).ScaleToFit(200, 200));
//                    document.Add(new Paragraph(ticket.Match.Thuisploeg + " VS " + ticket.Match.Uitploeg).SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD)).SetFontSize(18));
//                    document.Add(new Paragraph($"Starttijd: {ticket.Match.Startuur}\nStadion: {ticket.Match.StadionNaam}\nZone: {ticket.Zone.Naam}\nZitplaats: {ticket.StoeltjeId}\nPrijs: {ticket.Zone.Prijs}\nNaam eigenaar: {customer.Name}"));

//                    // Generate QR code
//                    string qrContent = "Ticket ID: " + ticket.TicketId + "\nToon dit QR-code bij de ingang met ID";
//                    Image qrCodeImage = new Image(ImageDataFactory.Create(BitmapToBytes(GenerateQRCode(qrContent))));
//                    document.Add(qrCodeImage.SetHorizontalAlignment(HorizontalAlignment.CENTER));

//                    document.Add(new Paragraph("\nToon deze QR-code samen met je ID om toegang te krijgen tot de wedstrijd.")
//                        .SetHorizontalAlignment(HorizontalAlignment.CENTER));

//                    document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE)); // Pagina-einde na elke ticket
//                }

//                document.Close();
//                return new MemoryStream(stream.ToArray());
//            }
//        }

//        private byte[] BitmapToBytes(System.Drawing.Bitmap img)
//        {
//            using (MemoryStream stream = new MemoryStream())
//            {
//                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
//                return stream.ToArray();
//            }
//        }

//        private System.Drawing.Bitmap GenerateQRCode(string qrContent)
//        {
//            QRCodeGenerator qrGenerator = new QRCodeGenerator();
//            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
//            QRCode qrCode = new QRCode(qrCodeData);
//            return qrCode.GetGraphic(5); // Grootte van 5 pixels
//        }
//    }
//}
