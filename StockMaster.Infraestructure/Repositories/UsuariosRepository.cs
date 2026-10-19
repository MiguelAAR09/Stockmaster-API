using StockMaster.Application.Interfaces;
using StockMaster.Domain;

namespace StockMaster.Infraestructure.Repositories;

public class UsuariosRepository : IUsuarioRepository
{
    private readonly StockMasterDbContext _context;

    public UsuariosRepository(StockMasterDbContext context)
    {
        _context = context;
    }

    public async Task<Usuario?> ObtenerUsuarioPorIdAsync(int id)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
    }
     public async Task<Usuario?> ObtenerUsuarioPorCorreoAsync(string correo)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Correo == correo);
    }
    public async Task<IEnumerable<Usuario>> ObtenerTodosLosUsuariosAsync()
    {
        return await _context.Usuarios.ToListAsync();
    }
    public async Task AgregarUsuarioAsync(Usuario usuario)
    {
        await _context.Usuarios.AddAsync(usuario);
    }
    public  Task ActualizarUsuarioAsync(Usuario usuario)
    {
         _context.Usuarios.Update(usuario);
         return Task.CompletedTask;
    }

    public async Task AgregarAdministradorAsync(Usuario administrador)
    {
        administrador.Rol = RolUsuario.Administrador;
        await _context.Usuarios.AddAsync(administrador);
    }

    //Poner al usuario como inactivo en lugar de eliminarlo
    public async Task DesactivarUsuarioAsync(int id)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == id);
        if (usuario != null)
        {
            usuario.Activo = false;
            _context.Usuarios.Update(usuario);
        }
    }
}
