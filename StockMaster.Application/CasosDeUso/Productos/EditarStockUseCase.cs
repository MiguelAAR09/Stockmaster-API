namespace StockMaster.Application.CasosDeUso.Productos;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;

public class EditarStockUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    // Inyectas una sola dependencia en lugar de múltiples repositorios
    public EditarStockUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Producto> EjecutarAsync(int id, int stockNuevo)
    {
        if (stockNuevo < 0)
        {
            throw new ArgumentException("El stock del producto no puede ser negativo.");
        }

        // Accedes al repositorio a través de _unitOfWork.Productos
        var productoExistente = await _unitOfWork.Productos.ObtenerPorIdAsync(id);
        if (productoExistente == null)
        {
            throw new KeyNotFoundException($"No existe un producto con el ID {id}.");
        }

        productoExistente.Stock = stockNuevo;

        if (productoExistente.Stock < productoExistente.StockMinimo)
        {
            var nuevaAlerta = new Alerta
            {
                ProductoId = productoExistente.Id,
                Descripcion = $"Alerta: El stock de '{productoExistente.Nombre}' bajó a {productoExistente.Stock} unidades.",
                Fecha = DateTime.UtcNow,
                Estado = EstadoAlerta.Pendiente
            };

            // Accedes al repositorio a través de _unitOfWork.Alertas
            await _unitOfWork.Alertas.AgregarAlertaAsync(nuevaAlerta);
        }

        await _unitOfWork.Productos.ActualizarAsync(productoExistente);

        // 🔥 Guarda el producto Y la alerta al mismo tiempo.
        // Si la alerta falla, la actualización del producto también se revierte.
        await _unitOfWork.SaveChangesAsync();

        return productoExistente;
    }
}