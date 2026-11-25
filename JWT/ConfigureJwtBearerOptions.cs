using abibas_back.JWT.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace abibas_back.JWT
{
    /// <summary>
    /// класс для параметров конфигурации токена
    /// </summary>
    public class ConfigureJwtBearerOptions : IConfigureNamedOptions<JwtBearerOptions>
    {
        private readonly IJWTOptions _jwtOptions;

        public ConfigureJwtBearerOptions(IOptions<AuthOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions.Value;
        }

        public void Configure(string? name, JwtBearerOptions options)
        {
            if (name != JwtBearerDefaults.AuthenticationScheme) return;

            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = _jwtOptions.ISSUER,
                ValidateAudience = true,
                ValidAudience = _jwtOptions.AUDIENCE,
                ValidateLifetime = true,
                IssuerSigningKey = _jwtOptions.GetSymmetricSecurityKey(),
                ValidateIssuerSigningKey = true
            };
        }

        public void Configure(JwtBearerOptions options) => Configure(Options.DefaultName, options);
    }
}
