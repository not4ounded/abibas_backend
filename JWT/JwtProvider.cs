using abibas_back.JWT.Interfaces;
using abibas_back.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace abibas_back.JWT
{
    /// <summary>
    /// Провайдер jwt токенов
    /// </summary>
    public class JwtProvider : IJWTProvider
    {
        private readonly IJWTOptions _jwtOptions;

        public JwtProvider(IOptions<IJWTOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public string GenerateAccessToken(UserItem user) 
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            // профиль пользователя для проверки
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            };

            // генерация токена
            var jwt = new JwtSecurityToken(
                issuer: _jwtOptions.ISSUER,
                audience: _jwtOptions.AUDIENCE,
                claims: claims,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                signingCredentials: new SigningCredentials(_jwtOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            // строка токена
            var tokenString = new JwtSecurityTokenHandler().WriteToken(jwt);

            return tokenString;
        }

        public KeyValuePair<string, RefreshTokenItem> GenerateRefreshToken(UserItem user)
        {
            if (user is null) throw new ArgumentNullException(nameof(user));

            string token = Guid.NewGuid().ToString();

            var refreshToken = new RefreshTokenItem()
            {
                TokenHash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token))),
                UserId = user.Id,
                ExpirationTime = DateTime.UtcNow.AddDays(7),
                CreatedAt = DateTime.UtcNow,
                IsRevoked = false
            };

            return new KeyValuePair<string, RefreshTokenItem>(token, refreshToken);
        }
    }
}   
