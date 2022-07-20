using EDHWebApp.Model;

namespace EDHWebApp.Data.TokenData;

public interface ITokenManager
{
    Task SetRefreshToke(string token);
    Task<string> GetRefreshToke();

    Task<CompanyUser> getUserBasedOnRefreshToken();
}