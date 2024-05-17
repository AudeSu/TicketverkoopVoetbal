using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas.Draw;
using iText.Layout;
using iText.Layout.Borders;
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
        public MemoryStream CreatePDFDocumentAsync(List<Ticket> tickets, List<Abonnement> abonnementen, string logoPath, string headerPath, AspNetUser user)
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
                .Add(new Text("Naar: " + user.FirstName + " " + user.LastName).SetBold().SetFontSize(20))
                .Add("\nEmail: " + user.Email)
                .Add("\nFactuurdatum: " + DateTime.Now.ToShortDateString())
                .Add("\nFactuurnummer: " + DateTime.Now.ToString("MMddHHmmss")));

            if (tickets.Count > 0)
            {
                document.Add(new Paragraph()
                    .Add(new Text("\n\nInformatie over de ticket(s):").SetBold())
                    .Add("\nBedankt voor uw aankoop van de ticket(s)! Om toegang te krijgen tot het stadion, raden we u aan om dit document af te drukken. Bij uw bezoek aan het stadion, laat de QR-code samen met uw identiteitsbewijs zien aan het toegangspersoneel. Op deze manier wordt u probleemloos toegelaten tot de wedstrijd."));
            }

            if (abonnementen.Count > 0)
            {
                document.Add(new Paragraph()
                    .Add(new Text("\n\nInformatie over je abonnement(en):").SetBold())
                    .Add("\nBedankt voor uw aankoop van je abonnement(en)! Door de aankoop van je abonnement van je club heb je toegang voor tot het stadion van alle thuismatchen."));
            }

            decimal totalPrice = 0;
            if (tickets.Count > 0)
            {
                document.Add(new Paragraph("\nTickets").SetFontSize(24));
                // Tabel tickets
                Table tableTickets = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
                tableTickets.AddHeaderCell("Thuisploeg");
                tableTickets.AddHeaderCell("Uitploeg");
                tableTickets.AddHeaderCell("Eigenaar");
                tableTickets.AddHeaderCell("Prijs");
                foreach (var ticket in tickets)
                {
                    tableTickets.AddCell(ticket.Match.Thuisploeg.Naam);
                    tableTickets.AddCell(ticket.Match.Uitploeg.Naam);
                    tableTickets.AddCell(ticket.FirstName + " " + ticket.LastName);
                    tableTickets.AddCell(ticket.Stoeltje.Zone.PrijsTicket.ToString("C"));
                    totalPrice += ticket.Stoeltje.Zone.PrijsTicket;
                }
                document.Add(tableTickets);
            }

            if (abonnementen.Count > 0)
            {
                document.Add(new Paragraph("\nAbonnementen").SetFontSize(24));
                // Tabel abonnementen
                Table tableAbonnement = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
                tableAbonnement.AddHeaderCell("Club");
                tableAbonnement.AddHeaderCell("Loopt van");
                tableAbonnement.AddHeaderCell("Tot");
                tableAbonnement.AddHeaderCell("Prijs");
                foreach (var abonnement in abonnementen)
                {
                    tableAbonnement.AddCell(abonnement.Club.Naam);
                    tableAbonnement.AddCell(abonnement.Seizoen.Startdatum.ToString("d MMMM yyyy"));
                    tableAbonnement.AddCell(abonnement.Seizoen.Einddatum.ToString("d MMMM yyyy"));
                    tableAbonnement.AddCell(abonnement.Stoeltje.Zone.PrijsAbonnement.ToString("C"));
                    totalPrice += abonnement.Stoeltje.Zone.PrijsAbonnement;
                }
                document.Add(tableAbonnement);
            }

            // Praragraaf met totaal
            Paragraph paragraph = new Paragraph("Totaal: " + totalPrice.ToString("C"))
                 .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                 .SetFontSize(15)
                 .SetFontColor(ColorConstants.BLACK)
                 .SetTextAlignment(TextAlignment.RIGHT);
            document.Add(paragraph);

            foreach (var ticket in tickets)
            {
                document.Add(new AreaBreak(AreaBreakType.NEXT_PAGE));

                var img = new iText.Layout.Element.Image(ImageDataFactory.Create(headerPath));
                document.Add(img);

                document.Add(new Paragraph("\n"));

                // Table for ticket details and QR code
                Table ticketTable = new Table(UnitValue.CreatePercentArray(new float[] { 3, 1 })).UseAllAvailableWidth();

                // Ticket details
                Cell ticketDetailsCell = new Cell()
                    .Add(new Paragraph($"{ticket.FirstName} {ticket.LastName}").SetBold().SetFontSize(20))
                    .Add(new Paragraph($"{ticket.Match.Thuisploeg.Naam.Trim()} - {ticket.Match.Uitploeg.Naam.Trim()}").SetFontSize(16).SetWordSpacing(2))
                    .Add(new Paragraph($"Datum: {ticket.Match.Datum:d MMMM yyyy}"))
                    .Add(new Paragraph($"Startuur: {ticket.Match.Startuur:hh\\:mm}"))
                    .Add(new Paragraph($"Stadion: {ticket.Match.Stadion.Naam}"))
                    .Add(new Paragraph($"Zone: {ticket.Stoeltje.Zone.Naam}"))
                    .Add(new Paragraph($"Stoeltje: {ticket.StoeltjeId}"))
                    .SetBorder(Border.NO_BORDER);

                ticketTable.AddCell(ticketDetailsCell);

                // Voeg QR-code toe met ticketinformatie
                string qrContent = "https://ticketverkoop.azurewebsites.net/";
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrContent, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(5);
                iText.Layout.Element.Image qrCodeImageElement = new iText.Layout.Element.Image(ImageDataFactory.Create(BitmapToBytes(qrCodeImage))).SetHorizontalAlignment(HorizontalAlignment.CENTER);
                Cell qrCodeCell = new Cell().Add(qrCodeImageElement).SetBorder(Border.NO_BORDER);

                ticketTable.AddCell(qrCodeCell);

                document.Add(ticketTable);

                document.Add(new Paragraph()
                    .Add(new Text("\n\nInformatie over de tickets:").SetBold())
                    .Add("\nBedankt voor uw aankoop van de tickets! Om toegang te krijgen tot het stadion, raden we u aan om dit document af te drukken. Bij uw bezoek aan het stadion, laat de QR-code samen met uw identiteitsbewijs zien aan het toegangspersoneel. Op deze manier wordt u probleemloos toegelaten tot de wedstrijd."));
            }

            document.Close();
            return new MemoryStream(stream.ToArray());
        }

        private static byte[] BitmapToBytes(Bitmap img)
        {
            using MemoryStream stream = new MemoryStream();
            img.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg);
            return stream.ToArray();
        }
    }
}
