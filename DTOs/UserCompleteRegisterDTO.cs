using abibas_back.Enums;
using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    /// <summary>
    /// User register
    /// </summary>
    public class UserCompleteRegisterDTO
    {
        [Required, EmailAddress(ErrorMessage = "Неккоректный формат Email")]
        public string Email { get; set; }
        [Required, Length(6, 6)] public string VerificationCode { get; set; }
    }
}
