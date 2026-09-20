namespace StockMaster.Application.DTOs;

using System.ComponentModel.DataAnnotations;

public class RegistrarClienteDTO
{
    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    public string Apellidos { get; set; } = string.Empty;

    [Required(ErrorMessage = "La identificación es obligatoria.")]
    [MinLength(7, ErrorMessage = "La identificación debe tener al menos 7 caracteres.")]
    public string NumeroIdentificacion { get; set; } = string.Empty;

    [Required(ErrorMessage = "El correo es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato de correo no es válido.")]
    public string Correo { get; set; } = string.Empty;

    [Required(ErrorMessage = "La contraseña es obligatoria.")]
    [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [MinLength(7, ErrorMessage = "El teléfono debe tener al menos 7 caracteres.")]
    public string Telefono { get; set; } = string.Empty;

    public string Direccion { get; set; } = string.Empty;

    public string CiudadResidencia { get; set; } = string.Empty;
}