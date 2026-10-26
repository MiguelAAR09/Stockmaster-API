namespace StockMaster.Infraestructure;
using Microsoft.EntityFrameworkCore;
using StockMaster.Domain;

public class StockMasterDbContext : DbContext
{

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
    .WithMany()
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
    .WithOne()
    .HasForeignKey(a => a.ProductoId);

    modelBuilder.Entity<Producto>()
    .Property(p => p.Activo)
    .HasDefaultValue(true);

    }

}
