namespace StockMaster.Application.CasosDeUso.Alertas;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class ConsultarRegistroAlertasUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public ConsultarRegistroAlertasUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<AlertaProductoDTO>> EjecutarAsync()
    {
        // 1. Obtener las alertas (el repositorio se encarga del .Include(a => a.Producto))
        var alertas = await _unitOfWork.Alertas.ObtenerTodasLasAlertasAsync();

        // 2. Mapear extrayendo el nombre del producto directamente
        var alertasDTO = alertas.Select(a => new AlertaProductoDTO
        {
            AlertaId = a.Id,
            ProductoId = a.ProductoId,
            NombreProducto = a.Producto?.Nombre ?? "Producto no asignado", // 👈 Asignación directa
            TipoAlerta = a.Descripcion,
            FechaGeneracion = a.Fecha,
            Estado = a.Estado.ToString(),

        });

        return alertasDTO;
    }
}