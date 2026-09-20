namespace StockMaster.Application.DTOs;

using System.ComponentModel.DataAnnotations;
using StockMaster.Domain;

public class ActualizarEstadoAlertaDTO
{
    [Required(ErrorMessage = "El ID de la alerta es obligatorio.")]
    public int AlertaId { get; set; }

    [Required(ErrorMessage = "El nuevo estado es obligatorio.")]
    public EstadoAlerta NuevoEstado { get; set; }
}