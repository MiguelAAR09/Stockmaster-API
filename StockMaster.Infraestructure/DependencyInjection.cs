using Microsoft.Extensions.DependencyInjection;
using StockMaster.Application.Interfaces;
using StockMaster.Infraestructure.Services;
using StockMaster.Infraestructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using StockMaster.Domain;

namespace StockMaster.Infraestructure;

public static class DependencyInjection 
{
    public static IServiceCollection AddInfraestructureServices(this IServiceCollection services,
    IConfiguration configuration)
    {
        //Base de datos
        services.AddDbContext<StockMasterDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        //Repositorios
        services.AddScoped<IUsuarioRepository, UsuariosRepository>();
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IPedidoRepository, PedidoRepository>();
        services.AddScoped<IProductoRepository, ProductoRepository>();

        //Middleware

        
        //Unit of work
        services.AddScoped<IUnitOfWork, UnitOfWork>(); 

        // Servicios stateless (Transient / singleton)
        services.AddTransient<IPasswordHasher, PasswordHasher>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddTransient<ITokenGenerator, TokenGenerator>();
        

        return services;
    }
}
