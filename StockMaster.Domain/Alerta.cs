namespace StockMaster.Domain;

public class Alerta
{
    public int Id { get; set; }
    public string Descripcion { get; set; } = string.Empty;

    public Producto? Producto { get; set; }
    public DateTime Fecha { get; set; }
    public int ProductoId { get; set; }
    public EstadoAlerta Estado { get; set; }

}

public enum EstadoAlerta
{
    Pendiente,
    En_revision,
    Resuelta
}