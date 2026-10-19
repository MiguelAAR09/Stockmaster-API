namespace StockMaster.Domain;

public class Pedido
{
    public int Id { get; set; }
    public int ClienteId{ get; set; }
    public DateTime FechaPedido { get; set; }
    public decimal Total { get; set; }
    public MetodoPago MetodoPago { get; set; }

    // Propiedad de navegación para el cliente asociado al pedido
    public Cliente  ? Cliente {get; set;}

    // Propiedad de navegación para los detalles del pedido
    public List<DetallePedido> Detalles { get; set; } = new List<DetallePedido>();
}

public enum MetodoPago
{
    Efectivo,
    Tarjeta,
    Transferencia

}