namespace StockMaster.Application.DTOs;

public class CrearPedidoDTO
{
    public int ClienteId { get; set; }
    public List<CrearDetallePedidoDTO> Detalles { get; set; } = new();
}