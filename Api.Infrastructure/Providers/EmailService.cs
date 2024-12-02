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
    //Todo: add settings with Ioption
    public async Task SendEmail(string toEmail, string subject, string body)
    {
        var fromAddress = new MailAddress(_config["EmailSettings:From"], "Ministerio de Cultura");
        var emailClient = new SmtpClient()
        {
            Host = _config["EmailSettings:Host"],
            Port = int.Parse(_config["EmailSettings:Port"]),
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(_config["EmailSettings:Username"], _config["EmailSettings:Password"])
        };
        var message = new MailMessage
        {
            From = fromAddress,
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(toEmail));
        await emailClient.SendMailAsync(message);
        _logger.LogWarning($"Sending email to {toEmail} from {_config["EmailSettings:From"]} with subject {subject}.", toEmail, _config["EmailSettings:From"], subject);
    }
}
