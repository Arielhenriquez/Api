namespace Api.Application.Interfaces;

public interface IEmailService
{
    Task SendTestEmail(string email);
    Task SendRequestConfirmationEmail(string collaboratorEmail, Guid requestId);
}
