using StockMaster.Domain;

namespace StockMaster.Application.DTOs;

public class RespuestaLoginDTO
{
    public string Token { get; set; } = string.Empty;
    public string TipoToken { get; set; } = "Bearer";
    public string Correo { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Rol { get; set; }  = string.Empty;

    
}

public enum RolUsuario
{
    Administrador,
    Cliente
}