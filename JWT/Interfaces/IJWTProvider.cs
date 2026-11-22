using abibas_back.DTOs;
using abibas_back.Models;

namespace abibas_back.JWT.Interfaces
{
    public interface IJWTProvider
    {
        string GenerateAccessToken(UserItem user);
        KeyValuePair<string, RefreshTokenItem> GenerateRefreshToken(UserItem user);
    }
}
