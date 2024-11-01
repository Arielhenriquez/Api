using Api.Application.Interfaces;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Providers;

public class EmailService : IEmailService
{
    public Task SendRequestConfirmationEmail(string collaboratorEmail, Guid requestId)
    {
        throw new NotImplementedException();
    }

    public async Task SendTestEmail()
    {
        // Configura los detalles del correo electrónico
        var subject = "Inventory Request Confirmation";
        var body = $"Your request with ID si has been successfully created.";

        //// Envía el correo (ejemplo usando MailKit)
        //using var smtpClient = new SmtpClient();
        //await smtpClient.ConnectAsync("smtp.mailtrap.io", 587, SecureSocketOptions.StartTls);
        //await smtpClient.AuthenticateAsync("MAILTRAP_USERNAME", "MAILTRAP_PASSWORD");

        //var message = new MimeMessage();
        //message.From.Add(MailboxAddress.Parse("noreply@company.com"));
        //message.To.Add(MailboxAddress.Parse(collaboratorEmail));
        //message.Subject = subject;
        //message.Body = new TextPart("plain") { Text = body };

        //await smtpClient.SendAsync(message);
        //await smtpClient.DisconnectAsync(true);
        var client = new SmtpClient("sandbox.smtp.mailtrap.io", 2525)
        {
            Credentials = new NetworkCredential("4a73f7c04e173c", "5ede0a8d794ca7"),
            EnableSsl = true
        };
        client.Send("from@example.com", "to@example.com", "Hello world", "testbody");
        

    }
}
