namespace StockMaster.Application.CasosDeUso.Alertas;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class ActualizarEstadoAlertasUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public ActualizarEstadoAlertasUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<RespuestaAlertaDTO> EjecutarAsync(ActualizarEstadoAlertaDTO dto)
    {
        Alerta alertaBuscada = await _unitOfWork.Alertas.ObtenerAlertaPorIdAsync(dto.AlertaId);

        if (alertaBuscada == null)
        {
            throw new InvalidOperationException("La alerta con el ID ingresado no existe.");
        }

        if (alertaBuscada.Estado > dto.NuevoEstado)
        {
            throw new InvalidOperationException("No se puede retroceder el estado de una alerta.");
        }

        alertaBuscada.Estado = dto.NuevoEstado;

        await _unitOfWork.Alertas.ActualizarAlertaAsync(alertaBuscada);
        await _unitOfWork.SaveChangesAsync();

        return new RespuestaAlertaDTO
        {
            AlertaId = alertaBuscada.Id,
            ProductoId = alertaBuscada.ProductoId,
            Descripcion = alertaBuscada.Descripcion,
            Estado = alertaBuscada.Estado.ToString(),
            Fecha = alertaBuscada.Fecha
        };
    }
}