namespace StockMaster.Application.CasosDeUso.Productos;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;
public class CrearProductoUseCase
{
    private readonly IProductoRepository _productoRepository;

    public CrearProductoUseCase(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<Producto> EjecutarAsync(ProductoDTO producto)
    {

        if(string.IsNullOrWhiteSpace(producto.Nombre) )
        {
            throw new ArgumentException("El nombre del producto no puede estar vacío.");
        }

        if(await _productoRepository.ObtenerPorNombreAsync(producto.Nombre) != null)
        {
            throw new ArgumentException("Ya existe un producto con ese nombre.");
        }  
        if(producto.PrecioUnitario <= 0)
        {
            throw new ArgumentException("El precio del producto no puede ser cero o negativo.");
        }

        if(producto.Stock < 0)
        {
            throw new ArgumentException("El stock del producto no puede ser negativo.");
        }
        if(producto.StockMinimo <= 0)
        {
            throw new ArgumentException("El stock minimo del producto no puede ser negativo.");
        }

        var nuevoProducto = new Producto
        {
            Nombre = producto.Nombre.Trim(),
            Categoria = producto.Categoria,
            PrecioUnitario = producto.PrecioUnitario,
            Stock = producto.Stock,
            StockMinimo = producto.StockMinimo
            // El "Id" no lo asignamos aquí porque la base de datos lo creará automáticamente
        };

        
        
        await _productoRepository.AgregarAsync(nuevoProducto);
        return nuevoProducto;
    }
}