namespace StockMaster.Application.CasosDeUso.Productos;

using StockMaster.Application.Interfaces;

public class EliminarProductoUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public EliminarProductoUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task EjecutarAsync(int id)
    {
        var eliminado = await _unitOfWork.Productos.EliminarPorIdAsync(id);
        if (!eliminado)
        {
            throw new KeyNotFoundException($"No existe un producto activo con el ID {id}.");
        }

        await _unitOfWork.SaveChangesAsync();
    }
}
