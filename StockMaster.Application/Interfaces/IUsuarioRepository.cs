namespace StockMaster.Application.Interfaces;

using StockMaster.Domain;


public interface IUsuarioRepository
{
    Task<Usuario?> ObtenerUsuarioPorIdAsync(int id);
    Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo);
    Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync();
    Task AgregarUsuarioAsync(Usuario usuario);
    Task ActualizarUsuarioAsync(Usuario usuario);

    Task AgregarAdministradorAsync(Usuario administrador);

    //Poner al usuario como inactivo en lugar de eliminarlo
    Task DesactivarUsuarioAsync(int id);
}