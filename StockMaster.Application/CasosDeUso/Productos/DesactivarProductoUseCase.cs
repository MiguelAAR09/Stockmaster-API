namespace StockMaster.Application.CasosDeUso.Productos;

using StockMaster.Application.Interfaces;

public class DesactivarProductoUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public DesactivarProductoUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task EjecutarAsync(int id)
    {
        // El filtro global excluye inactivos: un producto ya desactivado devuelve null
        var producto = await _unitOfWork.Productos.ObtenerPorIdAsync(id);
        if (producto == null)
        {
            throw new KeyNotFoundException($"No existe un producto activo con el ID {id}.");
        }

        producto.Activo = false;

        await _unitOfWork.Productos.ActualizarAsync(producto);
        await _unitOfWork.SaveChangesAsync();
    }
}
