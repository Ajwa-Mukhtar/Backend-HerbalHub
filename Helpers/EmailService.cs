using Microsoft.Extensions.Configuration;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

public class EmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        var smtpSettings = _config.GetSection("SmtpSettings");

        using var client = new SmtpClient(smtpSettings["Host"], int.Parse(smtpSettings["Port"]))
        {
            EnableSsl = bool.Parse(smtpSettings["EnableSsl"]),
            Credentials = new NetworkCredential(smtpSettings["Username"], smtpSettings["Password"])
        };

        var mailMessage = new MailMessage
        {
            From = new MailAddress(smtpSettings["Username"]),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };

        mailMessage.To.Add(toEmail);

        await client.SendMailAsync(mailMessage);
    }

    internal async Task SendEmailAsync(string email, object subject, string body)
    {
        throw new NotImplementedException();
    }

    internal async Task SendEmailAsync(object email, string subject, string body)
    {
        throw new NotImplementedException();
    }
}
