namespace StockMaster.Application.Interfaces;

using StockMaster.Domain;

public interface ITokenGenerator
{

    string GenerateToken(Usuario usuario);

}