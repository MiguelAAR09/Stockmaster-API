using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StockMaster.Infraestructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly StockMasterDbContext _context;

    public UnitOfWork(StockMasterDbContext context)
    {
        _context = context;
        Productos = new ProductoRepository(_context);
        Usuarios = new UsuariosRepository(_context);
        Pedidos = new PedidoRepository(_context);
        Alertas = new AlertaRepository(_context);
        Clientes = new ClienteRepository(_context);
    }

    public IProductoRepository Productos { get; private set; }
    public IUsuarioRepository Usuarios { get; private set; }
    public IPedidoRepository Pedidos { get; private set; }
    public IAlertaRepository Alertas { get; private set; }
    public IClienteRepository Clientes { get; private set; }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
        
            
     }

}
