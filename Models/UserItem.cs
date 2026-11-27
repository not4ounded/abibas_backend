using abibas_back.Enums;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace abibas_back.Models
{
    [Index(nameof(Email), IsUnique = true, Name = "IX_UserItem_Email")]
    public class UserItem
    {
        public long Id { get; set; }
        public string FirstName {  get; set; }
        public string LastName {  get; set; }
        [EmailAddress(ErrorMessage = "Введите корректный Email адрес")]
        public string Email { get; set; }
        public string PhoneNum {  get; set; }
        public string PasswordHash {  get; set; }
        public bool AgreeToTerms { get; set; }
        public RoleStatus Role { get; set; } = RoleStatus.User;
    }
}
