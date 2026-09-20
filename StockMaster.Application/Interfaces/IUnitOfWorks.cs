namespace StockMaster.Application.Interfaces;

public interface IUnitOfWork : IDisposable
{
    // Repositorios expuestos como propiedades de solo lectura
    IProductoRepository Productos { get; }
    IPedidoRepository Pedidos { get; }
    IAlertaRepository Alertas { get; }
    IClienteRepository Clientes { get; }
    IUsuarioRepository Usuarios { get; }

    /// <summary>
    /// Guarda todos los cambios realizados en los diferentes repositorios 
    /// en una sola transacción atómica. Si algo falla, nada se guarda en la BD.
    /// </summary>
    Task<int> SaveChangesAsync();
}