using Api.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace Api.Infrastructure.Providers;

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
        string hola = _config["EmailSettings:Username"];
        string hola2 = _config["EmailSettings:Password"];
        string hola3 = _config["EmailSettings:Host"];
        string hola4 = _config["EmailSettings:Port"];
        var fromAddress = new MailAddress(_config["EmailSettings:From"], "Ministerio de Cultura");
        var toAddress = new MailAddress(toEmail);

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
