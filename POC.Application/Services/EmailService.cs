using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using POC.Application.Interfaces;
using POC.Application.Settings;

namespace POC.Application.Services
{
  public class EmailService : IEmailService
  {
    private readonly SmtpSettings _smtpSettings;

    public EmailService(IOptions<SmtpSettings> smtpSettings)
    {
      _smtpSettings = smtpSettings.Value;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
      using (var client = new SmtpClient(_smtpSettings.Host, _smtpSettings.Port))
      {
        client.Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password);
        client.EnableSsl = _smtpSettings.EnableSsl;

        var mailMessage = new MailMessage
        {
          From = new MailAddress(_smtpSettings.From),
          Subject = subject,
          Body = body,
          IsBodyHtml = true,
        };

        mailMessage.To.Add(to);

        await client.SendMailAsync(mailMessage);
      }
    }
  }
}
