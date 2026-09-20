namespace StockMaster.Application.Interfaces;

using StockMaster.Domain;

public interface IProductoRepository
{
    Task<Producto> ObtenerPorIdAsync(int id);
    Task<Producto> ObtenerPorNombreAsync(string nombre);
    Task<IEnumerable<Producto>> ObtenerTodosAsync();
    Task AgregarAsync(Producto producto);
    Task ActualizarAsync(Producto producto);
    Task EliminarAsync(int id);

    Task<IEnumerable<Producto>> ObtenerProductosConBajoStockAsync();
}