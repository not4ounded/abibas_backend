using Microsoft.IdentityModel.Tokens;

namespace abibas_back.JWT.Interfaces
{
    public interface IJWTOptions
    {
        public const string SectionName = "Jwt";
        public string? ISSUER { get; set; }
        public string? AUDIENCE { get; set; }
        public string? SecretKey { get; set; }
        SymmetricSecurityKey GetSymmetricSecurityKey();
    }
}
