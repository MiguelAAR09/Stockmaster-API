
using StockMaster.Domain;
using StockMaster.Application.Interfaces;


namespace StockMaster.Infraestructure.Repositories;

public class PedidoRepository : IPedidoRepository
{

    private readonly StockMasterDbContext _context;
    public PedidoRepository(StockMasterDbContext context)
    {
        _context = context;
    }
    public async Task<Pedido?> ObtenerPedidoPorIdAsync(int id)
    {
        return await _context.Pedidos.FirstOrDefaultAsync(p => p.Id == id);
    }
    public async Task<IEnumerable<Pedido>> ObtenerPedidosPorClienteIdAsync(int clienteId)
    {
        return await _context.Pedidos.Where(p => p.ClienteId == clienteId ).ToListAsync();
    }
        public async  Task<IEnumerable<DetallePedido>> ObtenerDetallesPorPedidoIdAsync(int pedidoId)
        {
            return await _context.DetallesPedidos.Where(d => d.PedidoId == pedidoId).ToListAsync().ToListAsync();
        }
    
    public async Task<IEnumerable<Pedido>> ObtenerTodosAsync()
    {
        return await _context.Pedidos.ToListAsync();
    }
      public async  Task AgregarAsync(Pedido pedido)
    {

        await _context.Pedidos.AddAsync(pedido);
    }
    public async Task ActualizarAsync(Pedido pedido)
    {
        _context.Pedidos.Update(pedido);
    }
}
