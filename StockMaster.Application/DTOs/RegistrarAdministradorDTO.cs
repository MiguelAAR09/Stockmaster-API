namespace StockMaster.Application.DTOs;

using System.ComponentModel.DataAnnotations;
using StockMaster.Domain;

public class RegistrarAdministradorDTO
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo no es válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    // Se asigna por defecto como Administrador

}