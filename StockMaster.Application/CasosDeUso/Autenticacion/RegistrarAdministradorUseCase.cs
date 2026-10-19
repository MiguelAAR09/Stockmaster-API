namespace StockMaster.Application.CasosDeUso.Autenticacion;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class RegistrarAdministradorUseCase
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;

    public RegistrarAdministradorUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
    }

    public async Task<RespuestaRegistrarUsuarioDTO> EjecutarAsync(RegistrarAdministradorDTO dto)
    {
        var usuarioExistente = await _unitOfWork.Usuarios.ObtenerUsuarioPorCorreoAsync(dto.Correo);
        if (usuarioExistente != null)
        {
            throw new InvalidOperationException($"El correo '{dto.Correo}' ya se encuentra registrado.");
        }

        string contrasenaHasheada = _passwordHasher.Hash(dto.Password);

        var nuevoAdministrador = new Usuario
        {
            Nombre = dto.Nombre,
            Correo = dto.Correo,
            ContrasenaHash = contrasenaHasheada,
            Rol = Domain.RolUsuario.Administrador, // 👈 Nombre completo para romper la ambigüedad
            Activo = true
        };

        await _unitOfWork.Usuarios.AgregarAdministradorAsync(nuevoAdministrador);
        await _unitOfWork.SaveChangesAsync();

        return new RespuestaRegistrarUsuarioDTO
        {
            Id = nuevoAdministrador.Id,
            Nombre = nuevoAdministrador.Nombre,
            Correo = nuevoAdministrador.Correo,
            Rol = nuevoAdministrador.Rol.ToString(), // 👈 Casteo explícito
            Activo = nuevoAdministrador.Activo
        };
    }
}