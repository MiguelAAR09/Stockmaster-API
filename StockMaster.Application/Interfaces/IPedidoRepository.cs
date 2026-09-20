namespace StockMaster.Application.Interfaces;
using StockMaster.Domain;

public interface IPedidoRepository
{

    Task<Pedido?> ObtenerPedidoPorIdAsync(int id);
    Task<IEnumerable<Pedido>> ObtenerPedidosPorClienteIdAsync(int clienteId);
    Task<IEnumerable<DetallePedido>> ObtenerDetallesPorPedidoIdAsync(int pedidoId);
    Task<IEnumerable<Pedido>> ObtenerTodosAsync();
    Task AgregarAsync(Pedido pedido);
    Task ActualizarAsync(Pedido pedido);
    
    
}
