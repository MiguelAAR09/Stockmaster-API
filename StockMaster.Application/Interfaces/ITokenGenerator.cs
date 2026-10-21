namespace StockMaster.Application.Interfaces;

using StockMaster.Domain;

public interface ITokenGenerator
{

    string GenerateToken(int Id, string email, RolUsuario rol);

}