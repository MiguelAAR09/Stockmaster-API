namespace StockMaster.Application.CasosDeUso.Productos;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class EditarDetalleProductoUseCase
{
    private readonly IProductoRepository _productoRepository;

    public EditarDetalleProductoUseCase(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<Producto> EjecutarAsync(int id, ProductoDTO dto)
    {
        // 1. Verificar existencia
        var productoExistente = await _productoRepository.ObtenerPorIdAsync(id);
        if (productoExistente == null)
        {
            throw new KeyNotFoundException($"No existe un producto con el ID: {id}");
        }

        // 2. Validar nombre vacío
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException("El nombre del producto no puede estar vacío.");
        }

        // 3. Validar duplicados (Pertenecientes a OTRO producto)
        var productoConMismoNombre = await _productoRepository.ObtenerPorNombreAsync(dto.Nombre.Trim());
        if (productoConMismoNombre != null && productoConMismoNombre.Id != id)
        {
            throw new ArgumentException("Ya existe otro producto registrado con ese nombre.");
        }

        // 4. Validaciones numéricas
        if (dto.PrecioUnitario <= 0)
        {
            throw new ArgumentException("El precio del producto debe ser mayor a cero.");
        }

        if (dto.Stock < 0)
        {
            throw new ArgumentException("El stock del producto no puede ser negativo.");
        }

        if (dto.StockMinimo < 0)
        {
            throw new ArgumentException("El stock mínimo del producto no puede ser negativo.");
        }

        // 5. Actualizar la entidad existente (Tracked by EF Core)
        productoExistente.Nombre = dto.Nombre.Trim();
        productoExistente.Categoria = dto.Categoria;
        productoExistente.PrecioUnitario = dto.PrecioUnitario;
        productoExistente.Stock = dto.Stock;
        productoExistente.StockMinimo = dto.StockMinimo;

        // 6. Guardar cambios
        await _productoRepository.ActualizarAsync(productoExistente);
        
        return productoExistente;
    }
}