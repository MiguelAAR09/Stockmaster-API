using StockMaster.Application.Interfaces;
using StockMaster.Infraestructure;

// Registro de dependencias
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<RegistrarClientesUseCase>();