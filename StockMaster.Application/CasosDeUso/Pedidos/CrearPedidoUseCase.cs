namespace StockMaster.Application.CasosDeUso.Pedidos;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class CrearPedidoUseCase
{
    private readonly IUnitOfWork _unitOfWork;

    public CrearPedidoUseCase(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<Pedido> EjecutarAsync(CrearPedidoDTO dto)
    {
        // 1. Validaciones iniciales del DTO
        if (dto.Detalles == null || !dto.Detalles.Any())
        {
            throw new ArgumentException("El pedido debe contener al menos un producto.");
        }

        // 2. Validar existencia del cliente
        var cliente = await _unitOfWork.Clientes.ObtenerClientePorIdAsync(dto.ClienteId);
        if (cliente == null)
        {
            throw new KeyNotFoundException($"No existe un cliente con el ID {dto.ClienteId}");
        }

        if (dto.Detalles.Any(d => d.Cantidad <= 0))
        {
            throw new ArgumentException("La cantidad de un producto en el pedido debe ser mayor a cero.");
        }

        // 3. Agrupar por producto: si el usuario repite un producto en varias líneas,
        // el stock se valida contra la suma, no línea por línea.
        // Se suma en long para que cantidades enormes no desborden y pasen la validación.
        var cantidadesPorProducto = dto.Detalles
            .GroupBy(d => d.ProductoId)
            .ToDictionary(g => g.Key, g => g.Sum(d => (long)d.Cantidad));

        // 4. Cargar cada producto una sola vez
        var diccionarioProductos = new Dictionary<int, Producto>();

        foreach (var productoId in cantidadesPorProducto.Keys)
        {
            var productoBD = await _unitOfWork.Productos.ObtenerPorIdAsync(productoId);
            if (productoBD == null)
            {
                throw new KeyNotFoundException($"No existe un producto con el ID {productoId}");
            }

            diccionarioProductos.Add(productoBD.Id, productoBD);
        }

        // 5. FASE 1: VALIDACIÓN DE STOCK con la cantidad total pedida de cada producto
        foreach (var (productoId, cantidadTotal) in cantidadesPorProducto)
        {
            var producto = diccionarioProductos[productoId];

            if (cantidadTotal > producto.Stock)
            {
                throw new ArgumentException($"No hay suficiente stock para '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {cantidadTotal}");
            }
        }

        // 6. FASE 2: PROCESAMIENTO (Calculo de Total, Descuento de Stock, Alertas y Detalles)
        // Un detalle por producto, con la cantidad total pedida
        var nuevoPedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            FechaPedido = DateTime.UtcNow,
            Detalles = new List<DetallePedido>()
        };

        decimal totalPedido = 0;

        foreach (var (productoId, cantidadTotal) in cantidadesPorProducto)
        {
            // Reutilizamos el objeto Producto almacenado en memoria
            var producto = diccionarioProductos[productoId];

            // Ya validado contra el stock, que es int: la conversión no pierde datos
            var cantidad = (int)cantidadTotal;

            // a) Calcular Subtotal y sumar al Total
            var subtotal = producto.PrecioUnitario * cantidad;
            totalPedido += subtotal;

            // b) Descontar Stock
            producto.Stock -= cantidad;
            await _unitOfWork.Productos.ActualizarAsync(producto);

            // c) Alerta de stock crítico si aplica
            if (producto.Stock < producto.StockMinimo)
            {
                var nuevaAlerta = new Alerta
                {
                    ProductoId = producto.Id,
                    Descripcion = $"Alerta: El stock de '{producto.Nombre}' ha bajado a {producto.Stock} unidades tras el pedido.",
                    Fecha = DateTime.UtcNow,
                    Estado = EstadoAlerta.Pendiente
                };

                await _unitOfWork.Alertas.AgregarAlertaAsync(nuevaAlerta);
            }

            // d) Agregar el detalle a la lista del pedido
            nuevoPedido.Detalles.Add(new DetallePedido
            {
                ProductoId = producto.Id,
                Cantidad = cantidad,
                PrecioUnitario = producto.PrecioUnitario
            });
        }

        nuevoPedido.Total = totalPedido;

        // 7. Guardar el Pedido (EF Core guardará la cabecera y los detalles automáticamente)
        await _unitOfWork.Pedidos.AgregarAsync(nuevoPedido);

        // 8. 🔥 Confirmar todo en una sola transacción.
        // Si otro pedido modificó el stock de alguno de estos productos desde que se leyó,
        // lanza ConflictoConcurrenciaException y no se guarda nada.
        await _unitOfWork.SaveChangesAsync();

        return nuevoPedido;
    }
}