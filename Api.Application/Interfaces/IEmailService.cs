namespace Api.Application.Interfaces;

public interface IEmailService
{
    Task SendEmail(string toEmail, string subject, string body);
}
