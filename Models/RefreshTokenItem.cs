using System.ComponentModel.DataAnnotations;

namespace abibas_back.Models
{
    public class RefreshTokenItem
    {
        [Key]
        public long Id { get; set; }
        public string TokenHash { get; set; } = null!;
        public long UserId { get; set; }
        public UserItem User { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpirationTime { get; set; }
        public bool IsRevoked { get; set; }
    }
}
