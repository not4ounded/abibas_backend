using abibas_back.Models;

namespace abibas_back.DTOs
{
    public class WishListItemDTO
    {
        public long Id { get; set; }
        public long UserId { get; set; }
        public long ProductId { get; set; }
        public List<ProductItem> Products { get; set; }
    }
}
