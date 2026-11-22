using System.ComponentModel.DataAnnotations;

namespace abibas_back.Models
{
    public class VerificationCodeItem
    {
        public long Id { get; set; }
        [EmailAddress] public string Email { get; set; }
        [Length(6, 6)] public string CodeHash { get; set; }
        [Length(5, 5)] public int FailedAttempts = 0;
        public bool IsInvalidated { get; set; } = false;
        public DateTime ExpiresAt { get; set; }
    }
}
