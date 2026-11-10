using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    /// <summary>
    /// User register
    /// </summary>
    public class UserRegisterDTO
    {
        [Required(ErrorMessage = "Обязательное поле")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Обязательное поле")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Неккоректный формат Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [Phone(ErrorMessage = "Неверный формат номера телефона")]
        public string PhoneNum { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [MinLength(8, ErrorMessage = "Слишком короткий пароль")]
        public string Password { get; set; }

        [Range(typeof(bool), "true", "true", ErrorMessage = "Необходимо согласиться с условиями")]
        public bool AgreeToTerms {  get; set; }
    }
}
