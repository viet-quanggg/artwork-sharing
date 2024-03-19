using System.Net;
using System.Net.Mail;
using ArtworkSharing.Core.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace ArtworkSharing.Service.Services;

public class EmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;

    public EmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public Task SendEmailAsync(string ToEmail, string Subject, string Body, bool IsBodyHtml = false)
    {
        var MailServer = _configuration["EmailSettings:MailServer"];
        var FromEmail = _configuration["EmailSettings:FromEmail"];
        var Password = _configuration["EmailSettings:Password"];
        var Port = int.Parse(_configuration["EmailSettings:MailPort"]);
        var client = new SmtpClient(MailServer, Port)
        {
            Credentials = new NetworkCredential(FromEmail, Password),
            EnableSsl = true
        };
        var mailMessage = new MailMessage(FromEmail, ToEmail, Subject, Body)
        {
            IsBodyHtml = IsBodyHtml
        };
        return client.SendMailAsync(mailMessage);
    }
}