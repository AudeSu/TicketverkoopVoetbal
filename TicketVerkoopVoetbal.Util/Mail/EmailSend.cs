using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using TicketVerkoopVoetbal.Util.Mail.Interfaces;

namespace TicketVerkoopVoetbal.Util.Mail
{
    public class EmailSend : IEmailSend
    {
        private readonly EmailSettings _emailSettings;
        public EmailSend(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = new MailMessage(); // aanmaken van een mail-object
            mail.To.Add(new MailAddress(email));
            mail.From = new MailAddress("tickets.voetbal.league@gmail.com");
            mail.Subject = subject;
            mail.Body = message;
            mail.IsBodyHtml = true;
            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception)
            { throw; }
        }

        public async Task SendEmailAttachmentAsync(string email, Stream attachmentStream, string attachmentName, bool isBodyHtml = false)
        {
            var mail = new MailMessage(); // aanmaken van een mail-object
            mail.To.Add(new MailAddress(email));
            mail.From = new
            MailAddress("tickets.voetbal.league@gmail.com");
            mail.Subject = "Factuur Ticketverkoop Pro League";
            mail.Body = "Bedankt om te bestellen op onze website u vind de factuur en tickets in de onderstaande bijlage";
            mail.Attachments.Add(new Attachment(attachmentStream, attachmentName));
            mail.IsBodyHtml = true;
            try
            {
                using (var smtp = new SmtpClient(_emailSettings.MailServer))
                {
                    smtp.Port = _emailSettings.MailPort;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential(_emailSettings.Sender, _emailSettings.Password);
                    await smtp.SendMailAsync(mail);
                }
            }
            catch (Exception)
            { throw; }
        }
    }
}
