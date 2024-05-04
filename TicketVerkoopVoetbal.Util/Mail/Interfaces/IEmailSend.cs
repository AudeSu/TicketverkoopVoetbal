namespace TicketVerkoopVoetbal.Util.Mail.Interfaces
{
    public interface IEmailSend
    {
        Task SendEmailAsync(string email);
        Task SendEmailAttachmentAsync(string email, Stream attachmentStream, string attachmentName, bool isBodyHtml = false);
    }
}
