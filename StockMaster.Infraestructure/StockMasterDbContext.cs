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
    .HasForeignKey<Cliente>(c => c.UsuarioId);  // ¿Cuál es la FK en Cliente?

    modelBuilder.Entity<DetallePedido>()
    .HasOne(d => d.pedido)
    .WithMany(p => p.Detalles)
    .HasForeignKey(d => d.PedidoId);

    modelBuilder.Entity<DetallePedido>()
    .HasOne(d => d.Producto)
    .WithMany()
    .HasForeignKey(d => d.ProductoId);

    modelBuilder.Entity<Pedido>()
    .HasOne(p => p.Cliente)
    .WithMany()
    .HasForeignKey(p => p.ClienteId);
    
    modelBuilder.Entity<Producto>()
    .HasMany( p=> p.Alertas)
    .WithOne(a => a.Producto)
    .HasForeignKey(a => a.ProductoId);

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

    }

}
