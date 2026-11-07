namespace StockMaster.Infraestructure.Repositories;

using Microsoft.EntityFrameworkCore;
using StockMaster.Application.Interfaces;
using StockMaster.Domain;


public class ProductoRepository : IProductoRepository
{
    private readonly StockMasterDbContext _context;

    public ProductoRepository(StockMasterDbContext context)
    {
        _context = context;
    }

    //Obtener un producto por Id
    public async Task<Producto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Productos.FirstOrDefaultAsync(p => p.Id == id);
    }


    // Agregar un producto nuevo
    public async Task AgregarAsync(Producto producto)
    {
        await _context.Productos.AddAsync(producto);
    }

    // Actualizar datos de un producto
    public Task ActualizarAsync(Producto producto)
    {
        _context.Productos.Update(producto);
        return Task.CompletedTask;
    }

    // Consultar lista completa de productos
    public async Task<IEnumerable<Producto>> ObtenerTodosAsync()
    {
        return await _context.Productos.ToListAsync();
    }

    //Obtener un producto por nombre
    public async Task<Producto?> ObtenerPorNombreAsync(string nombre)
    {
        return await _context.Productos.FirstOrDefaultAsync(p => p.Nombre == nombre);
    }

    public async Task<IEnumerable<Producto>> ObtenerProductosConBajoStockAsync()
    {
        return await _context.Productos
            .Where(p => p.Stock < p.StockMinimo)
            .ToListAsync();
    }


}