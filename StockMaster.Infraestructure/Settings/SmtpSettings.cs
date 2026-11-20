using System.ComponentModel.DataAnnotations;

namespace StockMaster.Infraestructure.Settings;

/// <summary>
/// Representa la sección "SmtpSettings" de appsettings.json.
/// Se rellena automáticamente mediante el Options Pattern (ver DependencyInjection).
/// </summary>
public sealed class SmtpSettings
{
    public const string SectionName = "SmtpSettings";

    [Required]
    public string Server { get; set; } = string.Empty;

    [Range(1, 65535)]
    public int Port { get; set; } = 587;

    [Required]
    public string SenderName { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string SenderEmail { get; set; } = string.Empty;

    [Required]
    public string Username { get; set; } = string.Empty;

    [Required]
    public string Password { get; set; } = string.Empty;
}
