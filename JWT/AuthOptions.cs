using abibas_back.JWT.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace abibas_back.JWT
{
    /// <summary>
    /// класс для описания характеристик генерации ключа
    /// </summary>
    public class AuthOptions : IJWTOptions
    {
        public const string SectionName = "Jwt";
        [Required] public string? ISSUER { get; set; }  // издатель токена
        [Required] public string? AUDIENCE { get; set; }  // потребитель токена
        [Required] [MinLength(32)] public string? SecretKey { get; set; } // ключ
        public SymmetricSecurityKey GetSymmetricSecurityKey() => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
    }
}
