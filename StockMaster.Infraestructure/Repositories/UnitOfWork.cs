using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.Excepciones;
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
        try
        {
            return await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Se traduce a una excepción de Application para que los casos de uso no dependan de EF Core
            throw new ConflictoConcurrenciaException(
                "Los datos fueron modificados por otra operación. Vuelva a intentarlo.", ex);
        }
    }

}
