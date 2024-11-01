namespace Api.Application.Interfaces;

public interface IEmailService
{
    Task SendTestEmail();
    Task SendRequestConfirmationEmail(string collaboratorEmail, Guid requestId);
}
