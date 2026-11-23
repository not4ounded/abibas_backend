using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    public class UserPersonalInfoDTO
    {
        [Required] public string FirstName { get; set; }
        [Required] public string LastName { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Введите корректный Email адрес")]
        public string Email { get; set; }
        [Required] public string PhoneNum { get; set; }
    }
}
