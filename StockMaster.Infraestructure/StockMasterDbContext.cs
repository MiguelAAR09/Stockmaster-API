namespace StockMaster.Infraestructure;

using Microsoft.EntityFrameworkCore;
using StockMaster.Domain;

public class StockMasterDbContext : DbContext
{
    // Nombre del filtro de borrado lógico, para ignorarlo sin desactivar otros filtros
    public const string FiltroProductosActivos = "ProductosActivos";

    public StockMasterDbContext(DbContextOptions<StockMasterDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Producto> Productos { get; set; }
    public DbSet<Alerta> Alertas { get; set; }
    public DbSet<DetallePedido> DetallesPedidos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<Cliente> Clientes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Cliente>() //Relación entre cliente y usuario
        .HasOne(c => c.Usuario)      // ¿Qué propiedad de navegación tiene Cliente hacia Usuario?
        .WithOne()     // ¿Qué propiedad de navegación tiene Usuario hacia Cliente?
        .HasForeignKey<Cliente>(c => c.UsuarioId)  // ¿Cuál es la FK en Cliente?
                                                   // Restrict: con Cascade el borrado arrastraría al Cliente, pero sus Pedidos están en Restrict
                                                   // y fallaría a medias. Si hay pedidos, el borrado físico del Usuario debe fallar limpio.
        .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<DetallePedido>()
        .HasOne(d => d.pedido)
        .WithMany(p => p.Detalles)
        .HasForeignKey(d => d.PedidoId)
        .OnDelete(DeleteBehavior.Cascade); // Los detalles no existen sin su pedido: se borran con él

        modelBuilder.Entity<DetallePedido>()
        .HasOne(d => d.Producto)
        .WithMany()
        .HasForeignKey(d => d.ProductoId)
        .OnDelete(DeleteBehavior.Restrict); // Un producto vendido es parte del historial de pedidos

        modelBuilder.Entity<Pedido>()
        .HasOne(p => p.Cliente)
        .WithMany()
        .HasForeignKey(p => p.ClienteId)
        .OnDelete(DeleteBehavior.Restrict); // Los pedidos son historial

        modelBuilder.Entity<Producto>()
        .HasMany(p => p.Alertas)
        .WithOne(a => a.Producto)
        .HasForeignKey(a => a.ProductoId)
        .OnDelete(DeleteBehavior.Restrict); // Nada se borra físicamente por accidente; coherente con el borrado lógico

        modelBuilder.Entity<Producto>()
        .Property(p => p.Activo)
        .HasDefaultValue(true)
        .HasSentinel(true); // Sin esto, un INSERT con Activo = false omitiría la columna y la BD pondría true

        // Nombre único solo entre productos activos: el de un inactivo se puede reutilizar
        modelBuilder.Entity<Producto>()
        .HasIndex(p => p.Nombre)
        .IsUnique()
        .HasFilter("\"Activo\" = true");

        // Borrado lógico: toda consulta sobre Productos ignora los inactivos.
        // Para ver inactivos (historial, auditoría) usar .IgnoreQueryFilters([FiltroProductosActivos])
        modelBuilder.Entity<Producto>()
        .HasQueryFilter(FiltroProductosActivos, p => p.Activo);

        // Las alertas de un producto desactivado también se ocultan (siguen en la BD).
        // Mismo nombre: IgnoreQueryFilters([FiltroProductosActivos]) apaga ambos filtros
        modelBuilder.Entity<Alerta>()
        .HasQueryFilter(FiltroProductosActivos, a => a.Producto!.Activo);

        // Concurrencia optimista: Npgsql mapea esta propiedad sombra a la columna de sistema xmin
        // (no crea columna). Si dos operaciones descuentan stock del mismo producto a la vez,
        // la segunda falla con DbUpdateConcurrencyException en lugar de pisar a la primera.
        modelBuilder.Entity<Producto>()
        .Property<uint>("Version")
        .IsRowVersion();

        // Última barrera en la BD contra stock negativo, aunque falle la validación en código
        modelBuilder.Entity<Producto>()
        .ToTable(t =>
        {
            t.HasCheckConstraint("CK_Productos_Stock_NoNegativo", "\"Stock\" >= 0");
            t.HasCheckConstraint("CK_Productos_StockMinimo_NoNegativo", "\"StockMinimo\" >= 0");
        });

        modelBuilder.Entity<Usuario>()
        .HasIndex(u => u.Correo)
        .IsUnique();

        modelBuilder.Entity<Cliente>()
        .HasIndex(c => c.NumeroIdentificacion)
        .IsUnique();

        modelBuilder.Entity<Producto>()
        .Property(p => p.PrecioUnitario)
        .HasPrecision(18, 2);

        modelBuilder.Entity<DetallePedido>()
        .Property(d => d.PrecioUnitario)
        .HasPrecision(18, 2);

        modelBuilder.Entity<Pedido>()
        .Property(p => p.Total)
        .HasPrecision(18, 2);

    }


}
