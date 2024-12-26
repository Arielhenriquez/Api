using System.Net;
using System.Net.Mail;
using Api.Application.Interfaces;
using Azure;
using Azure.Communication.Email;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using EmailAddress = Azure.Communication.Email.EmailAddress;

namespace Api.Infrastructure.Providers;

public class EmailProvider : IEmailProvider
{
    private readonly ILogger<EmailProvider> _logger;
    private readonly IConfiguration _config;

    public EmailProvider(ILogger<EmailProvider> logger, IConfiguration config)
    {
        _logger = logger;
        _config = config;
    }
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

    public async Task SendEmailToSingleRecipient(string emailAddress, string subject, string body)
    {
        string connectionString = _config.GetConnectionString("EmailCommunicationService");
        var emailClient = new EmailClient(connectionString);


        var emailMessage = new EmailMessage(
            senderAddress: _config["EmailSettings:From"],
            content: new EmailContent(subject)
            {
                Html = body
            },
           recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(emailAddress) })
        );

        EmailSendOperation emailSendOperation = emailClient.Send(
            WaitUntil.Completed,
            emailMessage);
        _logger.LogWarning($"Sending email to {emailAddress} from {_config["EmailSettings:From"]} with subject {subject}.", emailAddress, _config["EmailSettings:From"], subject);
    }

    public async Task SendBulkEmail(List<string> emailAddresses, string subject, string body)
    {
        string connectionString = _config.GetConnectionString("EmailCommunicationService");
        var emailClient = new EmailClient(connectionString);

        var recipients = new List<EmailAddress>();
        foreach (var email in emailAddresses)
        {
            recipients.Add(new EmailAddress(email));
        }

        var emailMessage = new EmailMessage(
            senderAddress: _config["EmailSettings:From"],
            content: new EmailContent(subject)
            {
                Html = body
            },
            recipients: new EmailRecipients(recipients)
        );

        EmailSendOperation emailSendOperation = emailClient.Send(
            WaitUntil.Completed,
            emailMessage);
        _logger.LogWarning($"Sending email to {emailAddresses} from {_config["EmailSettings:From"]} with subject {subject}.", emailAddresses, _config["EmailSettings:From"], subject);
    }
}
