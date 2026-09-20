namespace StockMaster.Application.DTOs;

using StockMaster.Domain;
public class RespuestaRegistrarUsuarioDTO
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Rol { get; set; } 
    public bool Activo { get; set; }
}