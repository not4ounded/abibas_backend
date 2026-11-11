using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    public class UserPersonalInfoDTO
    {
        public long UserItemId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [EmailAddress(ErrorMessage = "Введите корректный Email адрес")]
        public string Email { get; set; }
        public string PhoneNum { get; set; }
    }
}
