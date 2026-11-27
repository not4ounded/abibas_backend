namespace abibas_back.Models
{
    public class PasswordResetTokenItem
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public UserItem User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public string TokenHash { get; set; } = null!;
    }
}
