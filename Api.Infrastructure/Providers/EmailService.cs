using Api.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Api.Infrastructure.Providers;

//Todo: Refactor this, move the replace to Create driver service and anyone who needs it
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;
    private readonly IConfiguration _config;

    public EmailService(ILogger<EmailService> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }

    public Task SendRequestConfirmationEmail(string collaboratorEmail, Guid requestId)
    {
        throw new NotImplementedException();
    }

    public async Task SendEmail(string toEmail, string subject, string driverName)
    {
        var fromAddress = new MailAddress(_config["EmailSettings:From"], "Ministerio de Cultura");
        var toAddress = new MailAddress(toEmail);

        //Todo refactor this to use it in add driver method with FileExtension.
        string templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "EmailTemplates", "CreatedDriversEmailTemplate.html");
        string htmlBody = await File.ReadAllTextAsync(templatePath);
        htmlBody = htmlBody.Replace("{{UserName}}", driverName);

        var message = new MailMessage
        {
            From = fromAddress,
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(toAddress);

        using var client = new SmtpClient
        {
            Host = _config["EmailSettings:Host"],
            Port = int.Parse(_config["EmailSettings:Port"]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_config["EmailSettings:Username"], _config["EmailSettings:Password"])
        };

        await client.SendMailAsync(message);
        _logger.Log(logLevel: LogLevel.Warning, "Correo enviado");
    }
}
