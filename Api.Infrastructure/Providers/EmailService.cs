using Api.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Api.Infrastructure.Providers;

public class EmailService : IEmailService
{
    public Task SendRequestConfirmationEmail(string collaboratorEmail, Guid requestId)
    {
        throw new NotImplementedException();
    }

    public Task SendTestEmail(string email)
    {
        throw new NotImplementedException();
    }
}
