using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    /// <summary>
    /// Users Login
    /// </summary>
    public class UserLoginDTO
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string Password { get; set; }
    }
}
