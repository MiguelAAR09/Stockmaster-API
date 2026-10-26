namespace StockMaster.Application.Interfaces;

using StockMaster.Domain;
public interface IAlertaRepository
{
    Task<Alerta?> ObtenerAlertaPorIdAsync(int id);
    Task<IEnumerable<Alerta>> ObtenerTodasLasAlertasAsync();
    Task AgregarAlertaAsync(Alerta alerta);
    Task ActualizarAlertaAsync(Alerta alerta);
    Task<IEnumerable<Alerta>> ObtenerAlertasPorProductoIdAsync(int id);

}