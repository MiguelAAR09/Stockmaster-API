namespace StockMaster.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class LoginDTO
{
    [Required(ErrorMessage = "El correo es necesario")]
    [EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es necesaria")]
    public string Contrasena { get; set; } = string.Empty;
}