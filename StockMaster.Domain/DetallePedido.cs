namespace StockMaster.Domain;

public class DetallePedido
{
    public int Id { get; set; }
    public int PedidoId { get; set; }
    public int ProductoId { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public Pedido ? pedido {get; set;}
    public Producto ? Producto {get; set;}
}       