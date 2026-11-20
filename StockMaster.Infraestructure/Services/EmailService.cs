
using Microsoft.Extensions.Options;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using StockMaster.Application.Interfaces;
using StockMaster.Infraestructure.Settings;
namespace StockMaster.Infraestructure.Services;

public class EmailService : IEmailService
{
    private readonly SmtpSettings _smtp;

    public EmailService(IOptions<SmtpSettings> options)
    {
        _smtp = options.Value;
    }

    public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_smtp.SenderName, _smtp.SenderEmail));

        email.To.Add(MailboxAddress.Parse(destinatario));
        email.Subject = asunto;

        var builder = new BodyBuilder();

        if (esHtml)
        {
            builder.HtmlBody = cuerpo;
        }
        else
        {
            builder.TextBody = cuerpo;
        }

        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        // Conexión segura usando STARTTLS
        await smtp.ConnectAsync(
            _smtp.Server,
            _smtp.Port,
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
            _smtp.Username,
            _smtp.Password);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }


}
