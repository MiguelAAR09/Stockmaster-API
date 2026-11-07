namespace StockMaster.Domain;

public class Usuario
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string ContrasenaHash { get; set; } = string.Empty;
    public RolUsuario Rol { get; set; }
    public bool Activo { get; set; } = true;
}

   /// Los valores son explícitos para que un rol sin asignar (0) sea el de menor privilegio.
// No reordenar ni renumerar: el valor se guarda como entero en la base de datos.

   public enum RolUsuario { Cliente = 0, Administrador = 1 }

