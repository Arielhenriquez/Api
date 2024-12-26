namespace Api.Application.Interfaces;

public interface IEmailProvider
{
    Task SendEmail(string toEmail, string subject, string body);
    Task SendEmailToSingleRecipient(string emailAddress, string subject, string body);
    Task SendBulkEmail(List<string> emailAddresses, string subject, string body);
}
