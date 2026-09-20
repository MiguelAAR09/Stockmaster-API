namespace StockMaster.Application.DTOs;

using StockMaster.Domain;

public class ProductoDTO
{
    public string Nombre { get; set; } = string.Empty;
    public CategoriaProducto Categoria { get; set; } 
    public decimal PrecioUnitario { get; set; }
    public int Stock { get; set; }
    public int StockMinimo { get; set; }
}

