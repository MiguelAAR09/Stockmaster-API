namespace StockMaster.Domain;

public class Producto
{
    public int Id { get; set; }
    public string Nombre { get; set; }
    public CategoriaProducto Categoria { get; set; }

    
    public decimal PrecioUnitario { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; }
    public bool Activo { get; set; } = true;

    public List<Alerta> Alertas { get; set; } = new List<Alerta>();
}


public enum CategoriaProducto
    {
        Tecnologia,
        Hogar,
        Moda,
        Alimentos,
        Entretenimiento
    }