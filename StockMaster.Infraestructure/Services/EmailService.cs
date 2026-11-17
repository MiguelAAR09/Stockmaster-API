
using Microsoft.Extensions.Configuration;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using StockMaster.Application.Interfaces;
namespace StockMaster.Infraestructure.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration config)
    {
        _configuration = config;
    }

    public async Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_configuration["SmtpSettings:SenderName"], _configuration["SmtpSettings:SenderEmail"]));

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
            _configuration["SmtpSettings:Server"],
            int.Parse(_configuration["SmtpSettings:Port"] ?? "587"),
            SecureSocketOptions.StartTls);

        await smtp.AuthenticateAsync(
             _configuration["SmtpSettings:Username"],
             _configuration["SmtpSettings:Password"]);

        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);
    }


}
