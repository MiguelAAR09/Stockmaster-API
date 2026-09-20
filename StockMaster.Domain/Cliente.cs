namespace StockMaster.Domain;

public class Cliente
{
    public int Id { get; set; } // Tu clave primaria para la base de datos
    public string NumeroIdentificacion { get; set; } = string.Empty; // Cédula, RUT, DNI, etc.
    public int UsuarioId { get; set; } // Conexión con la cuenta de Usuario
    public string Nombres { get; set; } = string.Empty;
    public string Apellidos { get; set; } = string.Empty;
    public string CorreoContacto { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Direccion { get; set; } = string.Empty;
    public string CiudadResidencia { get; set; } = string.Empty;

    public Usuario? Usuario { get; set; } // Relación con la entidad Usuario
}