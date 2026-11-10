using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    public class UserResetDTO
    {
        [Required(ErrorMessage = "Введите Email")]
        [EmailAddress(ErrorMessage = "Введите корректный Email адрес")]
        public string Email {  get; set; }
    }
}
