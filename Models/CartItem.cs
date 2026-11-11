namespace abibas_back.Models
{
    public class CartItem
    {
        public long UserId { get; set; }
        public UserItem User { get; set; }
        public long ProductId { get; set; }
        public ProductItem Product { get; set; }
    }
}
