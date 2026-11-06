namespace StockMaster.Application.CasosDeUso.Productos;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;

public class ConsultarProductosUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public ConsultarProductosUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Producto>> EjecutarAsync()
    {
        return await _unitOfWork.Productos.ObtenerTodosAsync();
    }
}