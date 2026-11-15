using System.ComponentModel.DataAnnotations;

namespace abibas_back.Models
{
    public class VerificationCodeItem
    {
        public long Id { get; set; }
        [EmailAddress] public string Email { get; set; }
        [Length(6, 6)] public string Code { get; set; }
        public DateTime ExpiresAt { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
