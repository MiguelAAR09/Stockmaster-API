namespace StockMaster.Application.Interfaces;

using StockMaster.Domain;
public interface IClienteRepository
{
    Task<Cliente?> ObtenerClientePorIdAsync(int id);

    Task<Cliente?> ObtenerClientePorCorreoAsync(string correo);
    Task<IEnumerable<Cliente>> ObtenerTodosLosClientesAsync();
    Task AgregarClienteAsync(Cliente cliente);
    Task ActualizarClienteAsync(Cliente cliente);

}