namespace StockMaster.Application.CasosDeUso.Autenticacion;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class RegistrarClienteUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher; // 👈 1. Inyectamos el hasher de contraseñas

    public RegistrarClienteUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<Cliente> EjecutarAsync(RegistrarClienteDTO dto)
    {
        // 2. Buscamos si el usuario ya existe en la tabla de Usuarios
        var usuarioExistente = await _unitOfWork.Usuarios.ObtenerUsuarioPorCorreoAsync(dto.Correo);
        
        // 3. Si NO es null, significa que el correo ya está registrado y debemos detener el flujo
        if (usuarioExistente != null)
        {
            throw new InvalidOperationException($"El correo '{dto.Correo}' ya se encuentra registrado.");
        }

        string contraseñaHasheada = _passwordHasher.Hash(dto.Password);

        var nuevoUsuario = new Usuario
        {
            Nombre = $"{dto.Nombres} {dto.Apellidos}",
            Correo = dto.Correo,
            ContrasenaHash = contraseñaHasheada,
            Rol = StockMaster.Domain.RolUsuario.Cliente,
            Activo = true
        };

        await _unitOfWork.Usuarios.AgregarUsuarioAsync(nuevoUsuario);

        var nuevoCliente = new Cliente
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            NumeroIdentificacion = dto.NumeroIdentificacion,
            Telefono = dto.Telefono,
            Direccion = dto.Direccion,
            CiudadResidencia = dto.CiudadResidencia,
            CorreoContacto = dto.Correo, 
            Usuario = nuevoUsuario // Asociamos el nuevo usuario al cliente
        };
        await _unitOfWork.Clientes.AgregarClienteAsync(nuevoCliente);
        
        await _unitOfWork.SaveChangesAsync();

        return nuevoCliente;
    }
}