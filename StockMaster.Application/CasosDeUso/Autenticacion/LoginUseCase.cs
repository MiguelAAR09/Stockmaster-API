namespace StockMaster.Application.CasosDeUso.Autenticacion;

using StockMaster.Domain;
using StockMaster.Application.Interfaces;
using StockMaster.Application.DTOs;

public class LoginUseCase
{
     private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher; // 👈 1. Inyectamos el hasher de contraseñas
    private readonly ITokenGenerator _tokenGenerator;

    public LoginUseCase(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator)
    {
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _tokenGenerator = tokenGenerator;
        
    }

    public async Task<RespuestaLoginDTO> EjecutarAsync(LoginDTO dto)
    {
        Usuario usuario = await _unitOfWork.Usuarios.ObtenerUsuarioPorCorreoAsync(dto.Correo);

        if(usuario == null){
            throw new InvalidOperationException($"El usuario ingresado no se encuentra registrado.");
        }

        if (!usuario.Activo)
        {
            throw new InvalidOperationException("El usuario no se encuentra activo en el sistema");
        }

        bool isValidPassword = _passwordHasher.Verify(dto.Contrasena, usuario.ContrasenaHash);
        if (!isValidPassword)
        {
            throw new InvalidOperationException("Credenciales inválidas.");
        }

        string token = _tokenGenerator.GenerateToken(usuario);

        RespuestaLoginDTO respuesta = new RespuestaLoginDTO
        {
            Nombre = usuario.Nombre,
            Token = token,
            Correo = usuario.Correo,
            Rol = usuario.Rol.ToString()
        };
        return respuesta;
    }
}