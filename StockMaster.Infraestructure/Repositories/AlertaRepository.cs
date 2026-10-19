using System;


using StockMaster.Application.Interfaces;
using StockMaster.Domain;
namespace StockMaster.Infraestructure.Repositories;

public class AlertaRepository : IAlertaRepository
{
    private readonly StockMasterDbContext _context;

    public AlertaRepository(StockMasterDbContext context)
    {
        _context = context;
    }

    public async Task<Alerta> ObtenerAlertaPorIdAsync (int id)
    {
        return await _context.Alertas.FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task AgregarAlertaAsync(Alerta alerta)
    {
        await _context.Alertas.AddAsync(alerta);

    }
    public async Task<IEnumerable<Alerta>> ObtenerAlertasPorProductoIdAsync(int productoId)
    {
        return await _context.Alertas.Where(a => a.ProductoId == productoId).ToListAsync();
    }

    public async Task<IEnumerable<Alerta>> ObtenerTodasLasAlertasAsync()
    {
        return await _context.Alertas.ToListAsync();
    }

    public Task ActualizarAlertaAsync(Alerta alerta)
    {
        _context.Alertas.Update(alerta);
        return Task.CompletedTask;
    }


}
