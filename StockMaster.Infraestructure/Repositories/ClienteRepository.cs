
using StockMaster.Application.Interfaces;
using StockMaster.Domain;

namespace StockMaster.Infraestructure.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly StockMasterDbContext _context;

    public ClienteRepository(StockMasterDbContext context)
    {
        _context = context;
    }
    public async Task<Cliente?> ObtenerClientePorIdAsync(int id)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<Cliente?> ObtenerClientePorCorreoAsync(string correo)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Correo == correo);
    }
    public async Task<IEnumerable<Cliente>> ObtenerTodosLosClientesAsync()
    {
        return await _context.Clientes.ToListAsync();
    }
    public async Task AgregarClienteAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
    }
    public Task ActualizarClienteAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        return Task.CompletedTask;
    }

}
