namespace StockMaster.Application.DTOs;

public class RespuestaAlertaDTO
{
    public int AlertaId { get; set; }
    public int ProductoId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}