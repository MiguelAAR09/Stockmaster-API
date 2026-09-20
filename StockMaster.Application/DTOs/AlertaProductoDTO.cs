namespace StockMaster.Application.DTOs;

public class AlertaProductoDTO
{
    public int AlertaId { get; set; }
    public int ProductoId { get; set; }

    public string NombreProducto { get; set; } = string.Empty;

    
    public string TipoAlerta { get; set; } = string.Empty; // ej. "Stock Bajo", "Agotado"
    public DateTime FechaGeneracion { get; set; }

    public string Estado { get; set; } = string.Empty;
}