
namespace StockMaster.Application.Interfaces;

public interface IEmailService
{
    Task EnviarCorreoAsync(string destinatario, string asunto, string cuerpo, bool esHtml = true);
}
