namespace abibas_back.Models
{
    public class WishlistItem
    {
        public long UserId {  get; set; }
        public UserItem User { get; set; } = null!;
        public long ProductId {  get; set; }
        public ProductItem Product { get; set; } = null!;
    }
}
