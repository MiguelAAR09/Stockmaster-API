namespace StockMaster.Application.CasosDeUso.Pedidos;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;

public class ConsultarPedidosUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public ConsultarPedidosUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Pedido>> EjecutarAsync()
    {
        return await _unitOfWork.Pedidos.ObtenerTodosAsync();
    }

}