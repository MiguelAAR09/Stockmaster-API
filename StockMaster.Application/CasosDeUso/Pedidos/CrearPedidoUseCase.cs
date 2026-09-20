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

        // 3. 🚀 CARGA PREVIA EN DICCIONARIO: Cargar los productos usando ObtenerPorIdAsync
        var diccionarioProductos = new Dictionary<int, Producto>();

        foreach (var detalle in dto.Detalles)
        {
            // Evitamos consultar a la BD dos veces el mismo producto si el usuario lo repitió en el DTO
            if (!diccionarioProductos.ContainsKey(detalle.ProductoId))
            {
                var productoBD = await _unitOfWork.Productos.ObtenerPorIdAsync(detalle.ProductoId);
                
                // Si el producto existe, lo guardamos en nuestro diccionario
                if (productoBD != null)
                {
                    diccionarioProductos.Add(productoBD.Id, productoBD);
                }
            }
        }

        // 4. FASE 1: VALIDACIÓN EN MEMORIA (Existencia, Cantidad y Stock)
        foreach (var detalle in dto.Detalles)
        {
            if (detalle.Cantidad <= 0)
            {
                throw new ArgumentException("La cantidad de un producto en el pedido debe ser mayor a cero.");
            }

            // Validar existencia desde el diccionario cargado
            if (!diccionarioProductos.TryGetValue(detalle.ProductoId, out var producto))
            {
                throw new KeyNotFoundException($"No existe un producto con el ID {detalle.ProductoId}");
            }

            // Validar stock disponible
            if (detalle.Cantidad > producto.Stock)
            {
                throw new ArgumentException($"No hay suficiente stock para '{producto.Nombre}'. Disponible: {producto.Stock}, solicitado: {detalle.Cantidad}");
            }
        }

        // 5. FASE 2: PROCESAMIENTO (Calculo de Total, Descuento de Stock, Alertas y Detalles)
        var nuevoPedido = new Pedido
        {
            ClienteId = dto.ClienteId,
            FechaPedido = DateTime.UtcNow,
            Detalles = new List<DetallePedido>()
        };

        decimal totalPedido = 0;

        foreach (var detalleDto in dto.Detalles)
        {
            // Reutilizamos el objeto Producto almacenado en memoria
            var producto = diccionarioProductos[detalleDto.ProductoId];

            // a) Calcular Subtotal y sumar al Total
            var subtotal = producto.PrecioUnitario * detalleDto.Cantidad;
            totalPedido += subtotal;

            // b) Descontar Stock
            producto.Stock -= detalleDto.Cantidad;
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
                Cantidad = detalleDto.Cantidad,
                PrecioUnitario = producto.PrecioUnitario
            });
        }

        nuevoPedido.Total = totalPedido;

        // 6. Guardar el Pedido (EF Core guardará la cabecera y los detalles automáticamente)
        await _unitOfWork.Pedidos.AgregarAsync(nuevoPedido);

        // 7. 🔥 Confirmar todo en una sola transacción
        await _unitOfWork.SaveChangesAsync();

        return nuevoPedido;
    }
}