using System.ComponentModel.DataAnnotations;

namespace abibas_back.DTOs
{
    public class UserInitRegisterDTO
    {
        [Required, EmailAddress] public string Email { get; set; }
    }
}
